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
        };

    }
}

interface Window { antGlobal: Ant.Global; }

window.antGlobal = new Ant.Global();
