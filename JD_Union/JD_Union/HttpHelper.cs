﻿/*
 * Created by SharpDevelop.
 * User: lzk
 * Date: 2014/8/21
 * Time: 21:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace JD_Union
{
	public enum CodeType
	{
		JavaScript,
		Url
	}
	
	/// <summary>
	/// Description of HttpHelper.
	/// </summary>
	public class HttpHelper
	{
		public CodeType CodeType
		{
			get;set;
		}
		public HttpHelper(CodeType codeType)
		{
			this.CodeType = codeType;
		}
		
		
		
		public string Request(string url, Dictionary<string, string> parameters, string method)
		{
			CookieContainer cc = LoadCookie();
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
			req.CookieContainer = cc;
			req.Method = method;
			if(string.IsNullOrEmpty(method))
			{
				method = "GET";
			}
			if(method == "POST")
			{
				req.ContentType = "application/x-www-form-urlencoded";
				//如果需要POST数据
				if (!(parameters == null || parameters.Count == 0))
				{
					StringBuilder buffer = new StringBuilder();
					int i = 0;
					foreach (string key in parameters.Keys)
					{
						if (i > 0)
						{
							buffer.AppendFormat("&{0}={1}", key, parameters[key]);
						}
						else
						{
							buffer.AppendFormat("{0}={1}", key, parameters[key]);
						}
						i++;
					}
					
					byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
					using (Stream stream = req.GetRequestStream())
					{
						stream.Write(data, 0, data.Length);
					}
				}
			}
			
			HttpWebResponse res = (HttpWebResponse) req.GetResponse();
			string content = "";
			using (StreamReader reader = new StreamReader( res.GetResponseStream()))
			{
				content = reader.ReadToEnd();
			}
			return content;
		}

		public string CreatePlacement(string itemUrl)
		{
			string url = "http://media.jd.com/gotoadv/createPlacement";
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("positionName","书籍");
			parameters.Add("siteName","搜奇买");
			parameters.Add("unionWebId","64738367");
			parameters.Add("materialType","7");
			parameters.Add("playType","7");
			parameters.Add("sizeType","2");
			parameters.Add("height","1");
			parameters.Add("width","1");
			parameters.Add("positionId","172123820");
			parameters.Add("type","1");
			parameters.Add("wareUrl",itemUrl);
			string content = Request(url, parameters, "POST");
			return content;
		}
		
		public string GetCustomCode(string placementId)
		{
			string codeType = "js";
			if(this.CodeType == CodeType.JavaScript)
			{
				codeType = "js";
			}
			else
			{
				codeType = "url";
			}
			
			string url = "http://media.jd.com/gotoadv/getCustomCode";
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("size", "0");
			parameters.Add("codeType", codeType);
			parameters.Add("placementId", placementId);
			string content = Request(url, parameters, "POST");
			return content;
		}
		
		CookieContainer LoadCookie()
		{
			CookieContainer container = new CookieContainer();
			string content = File.ReadAllText("cookies.txt");
			string[] lines = content.Split('\n');
			for (int i = 0; i < lines.Length; i++) {
				string cookieInfo = lines[i];
				if(cookieInfo.Trim() == "") {
					continue;
				}
				string[] parts = cookieInfo.Split(',');
				Dictionary<string, string> dic = new Dictionary<string, string>();
				for (int j = 0; j < parts.Length; j++) {
					if(parts[j].Trim() == "")
					{
						continue;
					}
					dic.Add(parts[j].Split(':')[0], parts[j].Split(':')[1]);
				}
				Cookie cookie = new Cookie(dic["name"], dic["value"], dic["path"], dic["domain"]);
				if(dic.ContainsKey("expirationDate"))
				{
					cookie.Expires = new DateTime(int.Parse(dic["expirationDate"].Split('.')[0]));
				}
				if(dic.ContainsKey("httpOnly"))
				{
					cookie.HttpOnly = bool.Parse(dic["httpOnly"]);
				}
				if(dic.ContainsKey("secure"))
				{
					cookie.Secure = bool.Parse(dic["secure"]);
				}
				
				container.Add(cookie);
			}
			return container;
		}
	}
}
