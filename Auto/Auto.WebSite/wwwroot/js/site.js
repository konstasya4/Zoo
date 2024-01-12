    //$(document).ready(connectToSignalR);
    //function displayNotification(user, json) {
    //    console.log(json);
    //var $target = $('div#signalr-notifications');
    //var data = JSON.parse(json);
    //var message =
    //    `NEW PRODUCT! <a href="/products/details/${data.serial}">${data.serial}</a> (${data.animalName} ${data.categoryName}, ${data.price}, ${data.title}).Name ${data.weightCode} ${data.name } `;
    //var $div = $(`< div > ${message} </div>`);
    //$target.prepend($div);
    //window.setTimeout(function () {$div.fadeOut(2000, function () { $div.remove(); }); }, 8000);
    //    }
    //function connectToSignalR() {
    //    console.log("Connecting to SignalR...");
    //window.notificationDivs = new Array();
    //var conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    //conn.on("DisplayNotification", displayNotification);
    //conn.start().then(function () {
    //    console.log("SignalR has started.");
    //        }).catch(function (err) {
    //    console.log(err);
    //        });
    //    }