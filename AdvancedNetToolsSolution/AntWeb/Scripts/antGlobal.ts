module Ant {

    export class Global {

        showNotification(message: string, notificationType: string) {
            var notification = $("#notification").data("kendoNotification");

            if (notification) {
                notification.show(message, notificationType);
            }
            else {
                alert(message);
            }
            if (notificationType === "error") {
                //this.addJSErrorSeleniumAttributeToBody(message);
            }
        }

        CreateRandomString(lengthOfString: number) {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (var i = 0; i < lengthOfString; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        }

    }
}

interface Window { antGlobal: Ant.Global; }

window.antGlobal = new Ant.Global();
