module Ant {

    export class Global {

        showNotification(message: string, notificationType: string) {
            var notification = $("#notification").data("kendoNotification");         
        
            notification.show(message, notificationType);
            if (notificationType === "error") {
                //this.addJSErrorSeleniumAttributeToBody(message);
            }
        };

    }
}

interface Window { antGlobal: Ant.Global; }

window.antGlobal = new Ant.Global();
