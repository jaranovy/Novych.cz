/*************************************************************/
/* Functions for Parni Cistic */
/*************************************************************/

function openDialog(e) {
    e.preventDefault();
    var myDialog;
    var myTitle;

    if ($(this).attr("data-type") === "create") {
        myDialog = $("#create-reservation-dialog");
        $("#Date_Create").val($(this).attr("data-date"));
        myTitle = "Nová rezervace";
    }
    else {
        myDialog = $("#delete-reservation-dialog");
        $("#Date_Delete").val($(this).attr("data-date"));
        myTitle = "Zrušení rezervace";
    }

    myDialog.dialog({
        autoOpen: false,
        resizable: false,
        width: 600,
        modal: true,
        title: myTitle,
        dialogClass: 'no-close-button-dialog'
    });

    if (myDialog.dialog("isOpen") === false) {
        myDialog.dialog("open");
    }
}


function refreshCalendar(data) {
    $("#calendar").html(data.calendar);
    $("#actual-month").html(data.actualMonth);
    $("#prev-month").attr("data-year", data.prevYear);
    $("#prev-month").attr("data-month", data.prevMonth);
    $("#next-month").attr("data-year", data.nextYear);
    $("#next-month").attr("data-month", data.nextMonth);

    $(".open-dialog").on("click", openDialog);
}

/*************************************************************/
/* Create Reservation Dialog */
/*************************************************************/

function CreateReservationOnSuccess(data) {
    // log("AJAX - CreateReservationOnSuccess");

    if (data.status === undefined) {
        $("#create-reservation-dialog").html(data);

        $("#create-dialog-close").on("click", function () {
            $("#create-reservation-dialog").dialog("close");
        });
    }
    else {
        $("#create-reservation-dialog").dialog("close");

        refreshCalendar(data, status);

        $("#create-status-dialog-msg").html(data.status);
        openCreateStatusDialog();
    }
}
function CreateReservationOnFailure(data) {
    // log("AJAX - CreateReservationOnFailure");
}
function CreateReservationOnBegin(data) {
    // log("AJAX - CreateReservationOnBegin");
}
function CreateReservationOnComplete(data) {
    // log("AJAX - CreateReservationOnComplete");
}

function openCreateStatusDialog() {
    // log("openCreateStatusDialog");

    var myDialog = $("#create-status-dialog");
    var myTitle = 'Nová rezervace';

    myDialog.dialog({
        autoOpen: false,
        resizable: false,
        modal: true,
        width: 600,
        title: myTitle,
        dialogClass: 'no-close-button-dialog'
    });

    if (myDialog.dialog("isOpen") === false) {
        myDialog.dialog("open");
    }
};

/*************************************************************/
/* Delete Reservation Dialog */
/*************************************************************/

function DeleteReservationOnSuccess(data, status) {
    //log("AJAX - DeleteReservationOnSuccess - " + data.type)

    if (data.status == undefined) {
        $("#delete-reservation-dialog").html(data);

        $("#delete-dialog-close").on("click", function () {
            $("#delete-reservation-dialog").dialog("close");
        });
    }
    else {
        $("#delete-reservation-dialog").dialog("close");

        refreshCalendar(data, status);

        $("#delete-status-dialog-msg").html(data.status);
        openDeleteStatusDialog();
    }
}
function DeleteReservationOnFailure(data, status) {
    //log("AJAX - DeleteReservationOnFailure")
}
function DeleteReservationOnBegin(data, status) {
    //log("AJAX - DeleteReservationOnBegin")
}
function DeleteReservationOnComplete(data, status) {
    //log("AJAX - DeleteReservationOnComplete")
}

function openDeleteStatusDialog() {
    // log("openDeleteStatusDialog");

    var myDialog = $("#delete-status-dialog");
    var myTitle = 'Zrušení rezervace';

    myDialog.dialog({
        autoOpen: false,
        resizable: false,
        width: 600,
        modal: true,
        title: myTitle,
        dialogClass: 'no-close-button-dialog'
    });

    if (myDialog.dialog("isOpen") === false) {
        myDialog.dialog("open");
    }
};

/*************************************************************/
/* Log functions */
/*************************************************************/

function addZero(x, n) {
    while (x.toString().length < n) {
        x = "0" + x;
    }
    return x;
}

function timestamp() {
    var d = new Date();
    var h = addZero(d.getHours(), 2);
    var m = addZero(d.getMinutes(), 2);
    var s = addZero(d.getSeconds(), 2);
    var ms = addZero(d.getMilliseconds(), 3);
    return h + ":" + m + ":" + s + ":" + ms;
}

function log(msg) {
    console.log("[" + timestamp() + "] " + msg);
}

$(document).ready(function () {

    $(".open-dialog").on("click", openDialog);

    $("#create-dialog-close").on("click", function () {
        $("#create-reservation-dialog").dialog("close");
    });

    $("#delete-dialog-close").on("click", function () {
        $("#delete-reservation-dialog").dialog("close");
    });

    $(".calendar-controls").click(function () {
        $.ajax({
            type: "POST",
            url: '/ParniCistic/getCalendar',
            data:

                JSON.stringify({
                    yearStr: $(this).attr("data-year"),
                    monthStr: $(this).attr("data-month")
                }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: refreshCalendar
        });
    });

    $("#create-status-dialog-close").on("click", function (e) {
        e.preventDefault();

        $("#create-status-dialog").dialog("close");
    });


    $("#delete-status-dialog-close").on("click", function (e) {
        e.preventDefault();

        $("#delete-status-dialog").dialog("close");
    });

    $.validator.methods.date = function (value, element) {
        return this.optional(element) || Date.parse(value, "dd.MM.yyyy") !== null;
    };
});

