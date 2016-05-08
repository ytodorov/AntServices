var Ant;
(function (Ant) {
    var Global = (function () {
        function Global() {
        }
        Global.prototype.showNotification = function (message, notificationType) {
            var notification = $("#notification").data("kendoNotification");
            if (notification) {
                notification.show(message, notificationType);
            }
            else {
                alert(message);
            }
            if (notificationType === "error") {
            }
        };
        Global.prototype.CreateRandomString = function (lengthOfString) {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < lengthOfString; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            return text;
        };
        return Global;
    })();
    Ant.Global = Global;
})(Ant || (Ant = {}));
window.antGlobal = new Ant.Global();
//# sourceMappingURL=antGlobal.js.map