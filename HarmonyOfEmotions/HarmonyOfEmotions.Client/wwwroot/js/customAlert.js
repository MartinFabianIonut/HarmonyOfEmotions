window.showCustomAlert = function (errorMessage) {
	var errorAlert = document.createElement('div');
	errorAlert.style.position = 'fixed';
	errorAlert.style.top = '20px';
	errorAlert.style.left = '50%';
	errorAlert.style.transform = 'translateX(-50%)';
	errorAlert.style.padding = '15px';
	errorAlert.style.background = 'linear-gradient(to right, #ff00cc, #333399)';
	errorAlert.style.color = 'white';
	errorAlert.style.borderRadius = '5px';
	errorAlert.style.boxShadow = '0 4px 6px rgba(0, 0, 0, 0.1)';
	errorAlert.style.zIndex = '9999';
	errorAlert.textContent = errorMessage;
	document.body.appendChild(errorAlert);
	setTimeout(function () {
		document.body.removeChild(errorAlert);
	}, 5000); // 5000 milliseconds = 5 seconds
}