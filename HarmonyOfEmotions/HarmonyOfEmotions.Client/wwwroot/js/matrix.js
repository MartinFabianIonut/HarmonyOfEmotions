window.getBoundingClientRect = function (element) {
	return element.getBoundingClientRect();
};

window.drawCircle = function (canvas, x, y) {
	var ctx = canvas.getContext('2d');
	ctx.clearRect(0, 0, canvas.width, canvas.height); // Clear the canvas
	drawMatrix(canvas); // Draw the matrix
	ctx.beginPath();
	if (screen.width < 768) {
		diameter = 2;
		stroke = 1;
	} else {
		diameter = 4;
		stroke = 2;
	}
	ctx.arc(x, y, diameter, 0, 2 * Math.PI); 
	

	ctx.lineWidth = stroke; // Set the stroke width
	ctx.strokeStyle = '#ab032d'; // Set the stroke color to match the fill color
	ctx.stroke(); // Draw the circle outline

	ctx.fillStyle = 'red'; // Change color as desired
	ctx.fill(); // Fill the circle
}

window.drawMatrix = function (canvas) {
	var ctx = canvas.getContext('2d');
	if (screen.width < 768) {
		width = screen.width / 1.3;
	} else
		if (screen.width < 830) {
			width = screen.width / 1.5;
		}
		else {
			width = screen.width / 2.5;
		}
	height = width * 0.75;
	canvas.width = width;
	canvas.height = height;

	// Clear the canvas
	ctx.clearRect(0, 0, canvas.width, canvas.height);

	// Define a gradient fill style
	var gradient = ctx.createLinearGradient(0, 0, canvas.width, canvas.height);
	gradient.addColorStop(0, '#FF6666');
	gradient.addColorStop(0.1, '#FFD966');
	gradient.addColorStop(0.2, '#CCFF66');
	gradient.addColorStop(0.3, '#66FF66');
	gradient.addColorStop(0.4, '#66FFCC');
	gradient.addColorStop(0.5, '#66FFFF');
	gradient.addColorStop(0.6, '#66CCFF');
	gradient.addColorStop(0.7, '#6666FF');
	gradient.addColorStop(0.8, '#CC66FF');
	gradient.addColorStop(0.9, '#FF66E6');

	// Fill the entire canvas with the gradient
	ctx.fillStyle = gradient;
	ctx.fillRect(0, 0, canvas.width, canvas.height);

	ctx.fillStyle = 'white';
	ctx.shadowColor = 'black';
	ctx.shadowOffsetX = -4;
	ctx.shadowOffsetY = 1;
	ctx.shadowBlur = 6;
	if (screen.width < 768) {
		ctx.font = '10px Bold Arial';
	} else {
		ctx.font = '20px Bold Arial';
	}

	// Write words at specific relative points based on canvas dimensions
	var textPositions = [
		{ text: 'Happy', x: width * 0.88, y: height * 0.45 },
		{ text: 'Pleased', x: width * 0.865, y: height * 0.57 },
		{ text: 'Confident', x: width * 0.65, y: height * 0.60 },
		{ text: 'Relaxed', x: width * 0.8, y: height * 0.75 },
		{ text: 'Calm', x: width * 0.75, y: height * 0.8 },
		{ text: 'Pensive', x: width * 0.55, y: height * 0.72 },
		{ text: 'Peaceful', x: width * 0.85, y: height * 0.88 },
		{ text: 'Sleepy', x: width * 0.6, y: height * 0.95 },
		{ text: 'Bored', x: width * 0.34, y: height * 0.9 },
		{ text: 'Melancholic', x: width * 0.4, y: height * 0.79 },
		{ text: 'Worried', x: width * 0.39, y: height * 0.6 },
		{ text: 'Depressed', x: width * 0.1, y: height * 0.73 },
		{ text: 'Anxious', x: width * 0.182, y: height * 0.885 },
		{ text: 'Sad', x: width * 0.02, y: height * 0.68 },
		{ text: 'Miserable', x: width * 0.01, y: height * 0.55 },
		{ text: 'Distressed', x: width * 0.05, y: height * 0.45 },
		{ text: 'Frustrated', x: width * 0.18, y: height * 0.375 },
		{ text: 'Annoyed', x: width * 0.3, y: height * 0.3 },
		{ text: 'Angry', x: width * 0.31, y: height * 0.225 },
		{ text: 'Afraid', x: width * 0.14, y: height * 0.16 },
		{ text: 'Tense', x: width * 0.35, y: height * 0.1 },
		{ text: 'Astonished', x: width * 0.6, y: height * 0.1 },
		{ text: 'Ambitious', x: width * 0.65, y: height * 0.25 },
		{ text: 'Excited', x: width * 0.86, y: height * 0.17 },
		{ text: 'Delighted', x: width * 0.825, y: height * 0.3 },
		{ text: 'Convinced', x: width * 0.7, y: height * 0.4 },
		{ text: 'Expectant', x: width * 0.55, y: height * 0.49 },
		{ text: 'Impatient', x: width * 0.4, y: height * 0.45 }
	];

	textPositions.forEach(function (pos) {
		ctx.fillText(pos.text, pos.x, pos.y);
	});
};