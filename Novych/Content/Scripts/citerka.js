/*************************************************************/
/* Functions for Citerka */
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

function createWad(pitch, len) {

    if (pitch === null) {
        pitch = 'A1';
    }
    if (len === null) {
        len = 1;
    }

    var wad = new Wad({
        source: 'square',
        pitch: pitch,
        volume: 1.4,
        env: {
            attack: .01 * len,
            decay: .005 * len,
            sustain: .2 * len,
            hold: .015 * len,
            release: .3 * len
        },
        filter: {
            type: 'lowpass',
            frequency: 1200,
            q: 8.5,
            env: {
                attack: .2,
                frequency: 600
            }
        }
    });


    return wad;
}

function getTone(toneStr) {

    /*log("getTone [" + toneStr + "]");*/

    var length = 1;
    if (toneStr.match(/^[A-Z0-9]+\+$/)) {
        length = 2;
        toneStr = toneStr.replace("+", "");
    }

    //    var base = 196; /* G3 as G1 */
    var base = 392; /* G4 as G1 */

    var multipler = 1;
    switch (toneStr) {
        case "X":
        case "DEL":
            tone = null;
            length = 0;
            break;
        case "G1":
            multipler = 1;
            break;
        case "A1":
            multipler = 1.12244898;
            break;
        case "H1":
            multipler = 1.260204082;
            break;
        case "C2":
            multipler = 1.334183673;
            break;
        case "D2":
            multipler = 1.49744898;
            break;
        case "E2":
            multipler = 1.681122449;
            break;
        case "FIS2":
            multipler = 1.887755102;
            break;
        case "G2":
            multipler = 2;
            break;
        case "A2":
            multipler = 2.244897959;
            break;
        case "H2":
            multipler = 2.520408163;
            break;
        case "C3":
            multipler = 2.670918367;
            break;
        case "D3":
            multipler = 2.99744898;
            break;
        case "E3":
            multipler = 3.364795918;
            break;
        case "FIS3":
            multipler = 3.775510204;
            break;
        case "G3":
            multipler = 4;
            break;
        default:
            tone = toneStr;
    }

    var tone = Math.round(base * multipler);

    return { tone: tone, length: length };
}

function playOneTone(toneStr) {

    var ret = getTone(toneStr);

    wad.stop('tone');
    wad.play({ pitch: ret.tone, label: 'tone' });
}

function erase() {

    log("erase");

    $("#HeaderString").val("");
    $("#NotesString").val("");
    $("#FooterString").val("");

    eraseTones();
}

function help() {

    log("help");

    $(window).scrollTop($('#help').offset().top);
}

function stopPlaying() {
    log("stopPlaying");

    $("#btn-play").val("Přehraj vše");
    song_playing = false;
    $(".dot").removeClass("classVisible");
    $(".dot").addClass("classHidden");
}

function startPlaying() {
    log("startPlaying");

    $("#btn-play").val("Zastav");
    song_playing = true;
    $(".dot").removeClass("classHidden");
    $(".dot").addClass("classVisible");
}

function playAll() {
    var notes = $("#tonesDiv").children();

    log("playAll notes [" + notes.length + "]");

    if (song_playing === true) {
        stopPlaying();
        /*log("song_playing " + song_playing);*/
        return;
    }

    wadsArray = [];

    var index;
    wadsArrayIndex = 0;

    for (index = 0; index < notes.length; ++index) {
        /*log("playAll " + index);*/

        if ($(notes[index]).hasClass("divSpacers")) {
            continue;
        }

        /*log("playAll " + index + ": [" + $(notes[index]).attr("data-tone") + "]");*/

        var ret = getTone($(notes[index]).attr("data-tone"));
        var toneLength = 1;
        if ($(notes[index]).attr("data-length") === "long") {
            toneLength = 2;
        }

        /*log("playAll " + index + ": [" + ret.tone + "][" + toneLength + "]");*/

        if (ret.tone !== null) {
            wadsArray.push({ wad: createWad(ret.tone, ret.length), len: toneLength, offset: $(notes[index]).offset() });
        }
    }


    startPlaying();
    /*log("song_playing " + song_playing);*/

    playWadsArray();
}

function playWadsArray() {

    /*log("playWadsArray " + wadsArrayIndex);*/

    if (song_playing !== true) {
        /*log("song_playing " + song_playing);*/
        return;
    }

    if (wadsArrayIndex >= wadsArray.length) {
        stopPlaying();
        /*log("song_playing " + song_playing);*/
        return;
    }

    wadsArray[wadsArrayIndex].wad.play();

    setTimeout(playWadsArray, wadsArray[wadsArrayIndex].len * 300);

    $(".dot").offset(wadsArray[wadsArrayIndex].offset);

    wadsArrayIndex += 1;
}

function eraseTones() {
    $("#tonesDiv").empty();

    var count = parseInt($("#tonesDiv").attr("data-count"))
    updateNotesCounter(-count);

    index = parseInt($("#tonesDiv").attr("data-id")) + 1;

    var newSpacer = $("<div id=\"spacerId_" + index + "\" class=\"divSpacers spacerNotSelected\">&nbsp;</div>");
    $("#tonesDiv").append(newSpacer);
    newSpacer.click(function () {
        log("Spacer clicked " + $(this).attr("id"));
        spacerSlected($(this));
    });
    spacerSlected(newSpacer);
}

function popTones() {
    log("popTones");

    eraseTones();

    var notesString = $("#NotesString").val();

    notesString = notesString.replace(/\s+/g, "").replace(/[,;]+/g, ",").replace(/,*$/, "");

    var notes = notesString.split(",");

    if (notes.length > 0) {
        notes.forEach(addOneTone);
    }
}

function addOneTone(item) {
    log("addOneTone [" + item + "]");

    if (item === "") {
        log("empty item");
        return;
    }

    if (updateNotesCounter(1) === false) {
        setFocusTonesDiv();
        return;
    }

    var lengthTone = "short";

    if (item.match(/\+/)) {
        lengthTone = "long";
        item = item.replace(/\+/, "");
    }

    index = parseInt($("#tonesDiv").attr("data-id")) + 1;
    $("#tonesDiv").attr("data-id", index);

    var newTone = $("<div id=\"toneId_" + index + "\" data-tone=\"" + item + "\" data-length=\"" + lengthTone + "\" class=\"divTones toneColor_" + item + " lengthTone_" + lengthTone + "\">" + item + "</div>");

    if ($(".spacerSelected").length === 0) {
        $("#tonesDiv").append(newTone);
    }
    else {
        $(".spacerSelected").after(newTone);
    }
    newTone.click(function () {
        log("Tone clicked " + $(this).attr("id"));
        switchToneLength($(this));
    });

    var newSpacer = $("<div id=\"spacerId_" + index + "\" class=\"divSpacers spacerNotSelected\">&nbsp;</div>");
    newTone.after(newSpacer);
    newSpacer.click(function () {
        log("Spacer clicked " + $(this).attr("id"));
        spacerSlected($(this));

    });

    spacerSlected(newSpacer);

    setFocusTonesDiv();
}

function updateNotesCounter(val) {
    /* log("updateNotesCounter [" + val + "]"); */

    var count = parseInt($("#tonesDiv").attr("data-count")) + val;

    if (count > 60) {
        $("#notesCounter").addClass("notesCounterError");

        $("#tonesDiv_valMsg").removeClass("field-validation-valid");
        $("#tonesDiv_valMsg").addClass("field-validation-error");
        $("#tonesDiv_valMsg").html("Překročen maximální počet Not: 60 not");

        return false;
    }

    if ($("#notesCounter").hasClass("notesCounterError")) {
        $("#tonesDiv_valMsg").removeClass("field-validation-error");
        $("#tonesDiv_valMsg").addClass("field-validation-valid");
        $("#tonesDiv_valMsg").html("");

        $("#notesCounter").removeClass("notesCounterError");
    }

    $("#tonesDiv").attr("data-count", count);
    $("#notesCounter").html("(" + count + "/60)");
    return true;
}

function spacerSlected(spacer) {
    $(".spacerSelected").removeClass("spacerSelected");

    spacer.removeClass("spacerNotSelected");
    spacer.addClass("spacerSelected");
}

function removePrevTone() {
    var spacer = $(".spacerSelected");
    var tone = spacer.prev();

    if (tone.length > 0) {
        var prevSpacer = tone.prev();

        spacer.remove();

        tone.remove();
        updateNotesCounter(-1);

        spacerSlected(prevSpacer);
    }
}

function removeNextTone() {
    var spacer = $(".spacerSelected");
    var tone = spacer.next();

    if (tone.length > 0) {
        var nextSpacer = tone.next();

        tone.remove();
        updateNotesCounter(-1);

        if (nextSpacer.length > 0) {
            nextSpacer.remove();
        }
    }
}

function selectPrevSpacer() {
    var spacer = $(".spacerSelected");

    var tone = spacer.prev("div");
    var prevSpacer = tone.prev("div");

    if (prevSpacer.length > 0) {
        spacerSlected(prevSpacer);
    }
}

function selectNextSpacer() {
    var spacer = $(".spacerSelected");

    var tone = spacer.next("div");

    if (tone.length > 0) {
        var nextSpacer = tone.next("div");

        if (nextSpacer.length > 0) {
            spacerSlected(nextSpacer);
        }
    }
}

function selectUpSpacer() {
    var spacer = $(".spacerSelected");

    var count = Math.round($("#tonesDiv").width() / (10 + 2 + 40 + 2));

    for (var i = 1; i <= count; i++) {
        var prevTone = spacer.prev("div");

        if (prevTone.length > 0) {
            var prevSpacer = prevTone.prev("div");

            if (prevSpacer.length === 0) {
                break;
            }

            spacer = prevSpacer;
        }
        else {
            break;
        }
    }

    spacerSlected(spacer);
}

function selectDownSpacer() {
    var spacer = $(".spacerSelected");

    var count = Math.round($("#tonesDiv").width() / (10 + 2 + 40 + 2));

    for (var i = 1; i <= count; i++) {
        var nextTone = spacer.next("div");

        if (nextTone.length > 0) {
            var nextSpacer = nextTone.next("div");

            if (nextSpacer.length === 0) {
                break;
            }

            spacer = nextSpacer;
        }
        else {
            break;
        }
    }

    spacerSlected(spacer);
}

function selectFirstSpacer() {
    var firstChild = $("#tonesDiv").children().first();

    spacerSlected(firstChild);
}

function selectLastSpacer() {
    var lastChild = $("#tonesDiv").children().last();

    spacerSlected(lastChild);
}

function setFocusTonesDiv() {
    $("#tonesDiv").focus();
}

function switchToneLength(tone) {
    log("switchToneLength");

    if (tone.hasClass("lengthTone_short")) {
        tone.removeClass("lengthTone_short");
        tone.addClass("lengthTone_long");
        tone.attr("data-length", "long");
    }
    else {
        tone.removeClass("lengthTone_long");
        tone.addClass("lengthTone_short");
        tone.attr("data-length", "short");
    }
}

/*************************************************************/
/* Global Variables */

var wad = createWad('A1', 1);
var wadsArray = [];
var wadsArrayIndex = 0;
var song_playing = false;

/*************************************************************/
/* Add functions to classes */

$(document).ready(function () {

    /* Click on Tone Keys */
    $(".keys").click(function () {
        var tone = $(this).attr('data-tone');

        log("keys click [" + tone + "]");

        if (tone === "DEL") {
            removePrevTone();
            setFocusTonesDiv();
            return;
        }

        addOneTone(tone);

        playOneTone(tone);
    });

    /* Click on Song list */
    $(".songLink").click(function () {
        var linkId = $(this).attr('id').replace('Link_', '');

        $("#HeaderString").val($("#HeaderText_".concat(linkId)).html());
        $("#NotesString").val($("#Notes_".concat(linkId)).html());
        $("#FooterString").val($("#FooterText_".concat(linkId)).html());

        popTones();

        $("form:first").submit();
    });

    /* #tonesDiv - keypress */
    $("#tonesDiv").keypress(function (e) {
        /* Home */
        if (e.which === 36 && !e.shiftKey) {
            return false;
        }

        /* End */
        if (e.which === 35 && !e.shiftKey) {
            return false;
        }

        /* Left */
        if (e.which === 37 && !e.shiftKey) {
            return false;
        }

        /* Right */
        if (e.which === 39 && !e.shiftKey) {
            return false;
        }

        /* Top */
        if (e.which === 38 && !e.shiftKey) {
            return false;
        }

        /* Down */
        if (e.which === 40 && !e.shiftKey) {
            return false;
        }
    });

    /* #tonesDiv - keyup */
    $("#tonesDiv").keyup(function (e) {
        /* Home */
        if (e.which === 36 && !e.shiftKey) {
            return false;
        }

        /* End */
        if (e.which === 35 && !e.shiftKey) {
            return false;
        }

        /* Left */
        if (e.which === 37 && !e.shiftKey) {
            return false;
        }

        /* Right */
        if (e.which === 39 && !e.shiftKey) {
            return false;
        }

        /* Top */
        if (e.which === 38 && !e.shiftKey) {
            return false;
        }

        /* Down */
        if (e.which === 40 && !e.shiftKey) {
            return false;
        }
    });

    /* #tonesDiv - keydown */
    $("#tonesDiv").keydown(function (e) {
        log("tonesDiv - keydown " + e.which);

        /* Enter */
        if (e.which === 13 && !e.shiftKey) {
            $("form:first").submit();
            return false;
        }

        /* Delete */
        if (e.which === 46 && !e.shiftKey) {
            removeNextTone();
            setFocusTonesDiv();
            return false;
        }

        /* Backspace */
        if (e.which === 8 && !e.shiftKey) {
            removePrevTone();
            setFocusTonesDiv();
            return false;
        }

        /* Left */
        if (e.which === 37 && !e.shiftKey) {
            selectPrevSpacer();
            setFocusTonesDiv();
            return false;
        }

        /* Right */
        if (e.which === 39 && !e.shiftKey) {
            selectNextSpacer();
            setFocusTonesDiv();
            return false;
        }


        /* Up */
        if (e.which === 38 && !e.shiftKey) {
            selectUpSpacer();
            setFocusTonesDiv();
            return false;
        }

        /* Down */
        if (e.which === 40 && !e.shiftKey) {
            selectDownSpacer();
            setFocusTonesDiv();
            return false;
        }

        /* Home */
        if (e.which === 36 && !e.shiftKey) {
            selectFirstSpacer();
            setFocusTonesDiv();
            return false;
        }

        /* End */
        if (e.which === 35 && !e.shiftKey) {
            selectLastSpacer();
            setFocusTonesDiv();
            return false;
        }
    });

    /* #GeNoPoCiForm - submit */
    $("#GeNoPoCiForm").submit(function () {
        var notes = $("#tonesDiv").children();

        var index;
        var buffer = "";
        var length = "";

        for (index = 0; index < notes.length; ++index) {
            if ($(notes[index]).hasClass("divSpacers")) {
                continue;
            }

            if ($(notes[index]).attr("data-length") === "long") {
                length = "+";
            }
            else {
                length = "";
            }

            buffer += $(notes[index]).attr("data-tone") + length + ", ";
        }

        $("#NotesString").val(buffer);
    });

    popTones();

    setFocusTonesDiv();
});