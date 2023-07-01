window.audioInterop = {
    playNotificationSound: function () {
        var audio = new Audio('/nioh_notification.mp3');
        audio.play();
    }
};