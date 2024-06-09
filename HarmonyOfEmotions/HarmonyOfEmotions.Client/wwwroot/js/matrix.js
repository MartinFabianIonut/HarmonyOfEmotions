window.getBoundingClientRect = function (element) {
	return element.getBoundingClientRect();
};

window.drawCircle = function (canvas, x, y) {
    var ctx = canvas.getContext('2d');
    ctx.clearRect(0, 0, canvas.width, canvas.height); // Clear the canvas
    drawMatrix(canvas); // Draw the matrix
    ctx.beginPath();
    ctx.arc(x, y, 5, 0, 2 * Math.PI);
    ctx.fillStyle = 'red'; // Change color as desired
    ctx.fill();
    ctx.stroke();
}

window.drawMatrix = function (canvas) {
    var ctx = canvas.getContext('2d');
    var width = canvas.width;
    var height = canvas.height;

    // Define a gradient fill style
    var gradient = ctx.createLinearGradient(0, 0, width, height);
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
    ctx.fillRect(0, 0, width, height);

    // Write words at specific fixed points
    var furiousX = Math.floor(width * 0.2); // 20% of width
    var furiousY = Math.floor(height * 0.8); // 80% of height
    ctx.fillStyle = 'white';
    ctx.font = '20px Arial';
    ctx.fillText('Furious', furiousX, furiousY);

    var happyX = Math.floor(width * 0.8); // 80% of width
    var happyY = Math.floor(height * 0.8); // 80% of height
    ctx.fillText('Happy', happyX, happyY);
};
