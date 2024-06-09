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

    ctx.fillStyle = 'white';
    ctx.font = '20px Bold Arial';
    // Write words at specific fixed points

    var happyX = Math.floor(width * 0.9);
    var happyY = Math.floor(height * 0.45); 
    ctx.fillText('Happy', happyX, happyY);

    var pleasedX = Math.floor(width * 0.88); 
    var pleasedY = Math.floor(height * 0.55); 
    ctx.fillText('Pleased', pleasedX, pleasedY);

    var confidentX = Math.floor(width * 0.65);
    var confidentY = Math.floor(height * 0.60);
    ctx.fillText('Confident', confidentX, confidentY);

    var relaxedX = Math.floor(width * 0.8);
    var relaxedY = Math.floor(height * 0.75);
    ctx.fillText('Relaxed', relaxedX, relaxedY);

    var calmX = Math.floor(width * 0.75);
    var calmY = Math.floor(height * 0.8);
    ctx.fillText('Calm', calmX, calmY);

    var pensiveX = Math.floor(width * 0.55);
    var pensiveY = Math.floor(height * 0.72);
    ctx.fillText('Pensive', pensiveX, pensiveY);

    var peacefulX = Math.floor(width * 0.87);
    var peacefulY = Math.floor(height * 0.85);
    ctx.fillText('Peaceful', peacefulX, peacefulY);

    var sleepyX = Math.floor(width * 0.6);
    var sleepyY = Math.floor(height * 0.95);
    ctx.fillText('Sleepy', sleepyX, sleepyY);

    var boredX = Math.floor(width * 0.34);
    var boredY = Math.floor(height * 0.9);
    ctx.fillText('Bored', boredX, boredY);

    var melancholicX = Math.floor(width * 0.4);
    var melancholicY = Math.floor(height * 0.79);
    ctx.fillText('Melancholic', melancholicX, melancholicY);

    var worriedX = Math.floor(width * 0.39);
    var worriedY = Math.floor(height * 0.6);
    ctx.fillText('Worried', worriedX, worriedY);

    var depressedX = Math.floor(width * 0.1);
    var depressedY = Math.floor(height * 0.73);
    ctx.fillText('Depressed', depressedX, depressedY);

    var anxiousX = Math.floor(width * 0.182);
    var anxiousY = Math.floor(height * 0.885);
    ctx.fillText('Anxious', anxiousX, anxiousY);

    var sadX = Math.floor(width * 0.02);
    var sadY = Math.floor(height * 0.68);
    ctx.fillText('Sad', sadX, sadY);

    var miserableX = Math.floor(width * 0.01);
    var miserableY = Math.floor(height * 0.55);
    ctx.fillText('Miserable', miserableX, miserableY);

    var distressedX = Math.floor(width * 0.05);
    var distressedY = Math.floor(height * 0.45);
    ctx.fillText('Distressed', distressedX, distressedY);

    var frustratedX = Math.floor(width * 0.18);
    var frustratedY = Math.floor(height * 0.375);
    ctx.fillText('Frustrated', frustratedX, frustratedY);

    var annoyedX = Math.floor(width * 0.3);
    var annoyedY = Math.floor(height * 0.3);
    ctx.fillText('Annoyed', annoyedX, annoyedY);

    var angryX = Math.floor(width * 0.31);
    var angryY = Math.floor(height * 0.225);
    ctx.fillText('Angry', angryX, angryY);

    var afraidX = Math.floor(width * 0.14);
    var afraidY = Math.floor(height * 0.16);
    ctx.fillText('Afraid', afraidX, afraidY);

    var tenseX = Math.floor(width * 0.35);
    var tenseY = Math.floor(height * 0.1);
    ctx.fillText('Tense', tenseX, tenseY);

    var astonishedX = Math.floor(width * 0.6);
    var astonishedY = Math.floor(height * 0.1);
    ctx.fillText('Astonished', astonishedX, astonishedY);

    var ambitiousX = Math.floor(width * 0.65);
    var ambitiousY = Math.floor(height * 0.25);
    ctx.fillText('Ambitious', ambitiousX, ambitiousY);

    var excitedX = Math.floor(width * 0.88);
    var excitedY = Math.floor(height * 0.2);
    ctx.fillText('Excited', excitedX, excitedY);

    var delightedX = Math.floor(width * 0.825);
    var delightedY = Math.floor(height * 0.3);
    ctx.fillText('Delighted', delightedX, delightedY);

    var convincedX = Math.floor(width * 0.7);
    var convincedY = Math.floor(height * 0.4);
    ctx.fillText('Convinced', convincedX, convincedY);

    var expectantX = Math.floor(width * 0.55);
    var expectantY = Math.floor(height * 0.45);
    ctx.fillText('Expectant', expectantX, expectantY);

    var impatientX = Math.floor(width * 0.4);
    var impatientY = Math.floor(height * 0.45);
    ctx.fillText('Impatient', impatientX, impatientY);
};
