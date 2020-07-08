window.canvas = {
	setSize: (canvasElement, width, height) => {
		canvasElement.width = width;
		canvasElement.height = height;
	},

	clear: (canvasElement, color) => {
		let context = canvasElement.getContext("2d");
		context.fillStyle = color;
		context.fillRect(0, 0, canvasElement.width, canvasElement.height);
	},

	drawLine: (canvasElement, pen, x1, y1, x2, y2) => {
		let context = canvasElement.getContext("2d");
		context.strokeStyle = pen;
		context.beginPath();
		context.moveTo(x1, y1);
		context.lineTo(x2, y2);
		context.stroke();
	},

	drawRectangle: (canvasElement, pen, x, y, width, height) => {
		let context = canvasElement.getContext("2d");
		context.strokeStyle = pen;
		context.beginPath();
		context.rect(x, y, width, height);
	},

	fillRectangle: (canvasElement, fill, x, y, width, height) => {
		let context = canvasElement.getContext("2d");
		context.fillStyle = fill;
		context.fillRect(x, y, width, height);
	},

	drawArc: (canvasElement, pen, x, y, width, height, startAngle, sweepAngle) => {
		let context = canvasElement.getContext("2d");
		context.strokeStyle = pen;
		context.beginPath();
		let xRadius = width / 2;
		let yRadius = height / 2;
		context.ellipse(x + xRadius, y + yRadius, xRadius, yRadius, 0, Math.PI * startAngle / 180, Math.PI * (startAngle + sweepAngle) / 180);
		context.stroke();
	},

	drawEllipse: (canvasElement, pen, x, y, width, height) => {
		let context = canvasElement.getContext("2d");
		context.strokeStyle = pen;
		context.beginPath();
		let xRadius = width / 2;
		let yRadius = height / 2;
		context.ellipse(x + xRadius, y + yRadius, xRadius, yRadius, 0, 0, 2 * Math.PI);
		context.stroke();
	},

	fillPolygon: (canvasElement, fill, points) => {
		let context = canvasElement.getContext('2d');
		context.fillStyle = fill;
		context.beginPath();
		let point = points[0];
		context.moveTo(point.x, point.y);
		for (let i = 0; i < points.length; i++) {
			let point = points[i];
			context.lineTo(point.x, point.y);
		}
		context.closePath();
		context.fill();
	}
};