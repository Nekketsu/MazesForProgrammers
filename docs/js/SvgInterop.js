window.showMessage = (message) => {
	alert(message);
};

window.svg = {
	setSize: (svgElement, width, height) => {
		svgElement.setAttribute("viewBox", `0 0 ${width} ${height}`);
		svgElement.setAttribute("width", `${width}px`);
		svgElement.setAttribute("height", `${height}px`);
	},

	clear: (svgElement, color) => {
		let svgns = "http://www.w3.org/2000/svg";
		let rect = document.createElementNS(svgns, "rect");
		rect.setAttribute("x", "0");
		rect.setAttribute("y", "0");
		rect.setAttribute("width", "100%");
		rect.setAttribute("height", "100%");
		rect.setAttribute("fill", color);
		rect.setAttribute("stroke", color);
		rect.setAttribute("vector-effect", "non-scaling-stroke");

		svgElement.innerHTML = "";

		svgElement.appendChild(rect);
	},

	drawLine: (svgElement, pen, x1, y1, x2, y2) => {
		let svgns = "http://www.w3.org/2000/svg";
		let line = document.createElementNS(svgns, "line");
		line.setAttribute("x1", x1);
		line.setAttribute("y1", y1);
		line.setAttribute("x2", x2);
		line.setAttribute("y2", y2);
		line.setAttribute("stroke", pen);
		line.setAttribute("vector-effect", "non-scaling-stroke");

		svgElement.appendChild(line);
	},

	drawRectangle: (svgElement, pen, x, y, width, height) => {
		let svgns = "http://www.w3.org/2000/svg";
		let rect = document.createElementNS(svgns, "rect");
		rect.setAttribute("x", x);
		rect.setAttribute("y", y);
		rect.setAttribute("width", width);
		rect.setAttribute("height", height);
		rect.setAttribute("stroke", pen);
		rect.setAttribute("fill", "none");
		rect.setAttribute("vector-effect", "non-scaling-stroke");

		svgElement.appendChild(rect);
	},

	fillRectangle: (svgElement, fill, x, y, width, height) => {
		let svgns = "http://www.w3.org/2000/svg";
		let rect = document.createElementNS(svgns, "rect");
		rect.setAttribute("x", x);
		rect.setAttribute("y", y);
		rect.setAttribute("width", width);
		rect.setAttribute("height", height);
		rect.setAttribute("fill", fill);
		rect.setAttribute("stroke", fill);
		rect.setAttribute("vector-effect", "non-scaling-stroke");

		svgElement.appendChild(rect);
	},

	drawArc: (svgElement, pen, x, y, width, height, startAngle, sweepAngle) => {
		let radius = {
			x: width / 2,
			y: height / 2
		};

		let center = {
			x: x + radius.x,
			y: y + radius.y
		};

		let start = {
			x: center.x + radius.x * Math.cos(startAngle * Math.PI / 180),
			y: center.y + radius.y * Math.sin(startAngle * Math.PI / 180)
		};

		let end = {
			x: center.x + radius.x * Math.cos((startAngle + sweepAngle) * Math.PI / 180),
			y: center.y + radius.y * Math.sin((startAngle + sweepAngle) * Math.PI / 180)
		};

		let largArcFlag = sweepAngle <= 180 ? 0 : 1;

		let d = `M ${start.x} ${start.y} A ${radius.x} ${radius.y} 0 ${largArcFlag} 1 ${end.x} ${end.y}`;


		let svgns = "http://www.w3.org/2000/svg";
		let arc = document.createElementNS(svgns, "path");
		arc.setAttribute("d", d);
		arc.setAttribute("fill", "none");
		arc.setAttribute("stroke", pen);
		arc.setAttribute("vector-effect", "non-scaling-stroke");

		svgElement.appendChild(arc);
	},

	drawEllipse: (svgElement, pen, x, y, width, height) => {
		let svgns = "http://www.w3.org/2000/svg";
		let ellipse = document.createElementNS(svgns, "ellipse");
		ellipse.setAttribute("cx", x + width / 2);
		ellipse.setAttribute("cy", y + height / 2);
		ellipse.setAttribute("rx", width / 2);
		ellipse.setAttribute("ry", height / 2);
		ellipse.setAttribute("stroke", pen);
		ellipse.setAttribute("fill", "none");
		ellipse.setAttribute("vector-effect", "non-scaling-stroke");

		svgElement.appendChild(ellipse);
	},

	fillPolygon: (svgElement, fill, points) => {
		let svgns = "http://www.w3.org/2000/svg";
		let polygon = document.createElementNS(svgns, "polygon");
		polygon.setAttribute("fill", fill);
		polygon.setAttribute("stroke", fill);

		for (point of points) {
			let svgPoint = svgElement.createSVGPoint();
			svgPoint.x = point.x;
			svgPoint.y = point.y;
			polygon.points.appendItem(svgPoint);
		}

		svgElement.appendChild(polygon);
	}
};