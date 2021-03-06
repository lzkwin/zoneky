(function(window, undefined) {
	window.onload = function() {
		// generateElements();
		waterfall();
	}

	function generateElements() {
		var container = document.getElementById('container');
		for (var i = 0; i < 22; i++) {
			var box = document.createElement('div');
			box.className = 'box';
			box.innerHTML = '<div class="pic"><img src="images/' + (i + 1) + '.jpg" /></div>';
			container.appendChild(box);
		};
	}

	function waterfall() {
		var container = document.getElementById('container');
		var targets = getElementsByClass(container, 'box');
		var boxWidth = targets[0].offsetWidth;
		var cols = ~~ (document.documentElement.clientWidth / boxWidth);
		container.style.cssText = 'width: ' + cols * boxWidth + 'px; margin: 15px auto;';

		var hArr = [];
		for (var i = 0; i < targets.length; i++) {
			if (i < cols) {
				hArr.push(targets[i].offsetHeight);
			} else {
				// var min = findMin(hArr);
				// var minH = min.value, index = min.index;
				var minH = Math.min.apply(null, hArr),
					index = hArr.indexOf(minH);
				targets[i].style.position = 'absolute';
				targets[i].style.top = minH + 'px';
				targets[i].style.left = boxWidth * index + 'px';
				hArr[index] = minH + targets[i].offsetHeight;
			}
		};
		console.log(hArr);
	}

	function findMin(array) {
		var min = array[0],
			index = 0;
		for (var i = 0; i < array.length; i++) {
			if (array[i] < min) {
				min = array[i];
				index = i;
			}
		};
		return {
			value: min,
			index: index
		};
	}

	function getElementsByClass(parent, className) {
		var children = parent.getElementsByTagName('*'),
			targets = [];
		for (var i = 0; i < children.length; i++) {
			if (children[i].className == className) {
				targets.push(children[i]);
			}
		};
		return targets;
	}
})(window, undefined);