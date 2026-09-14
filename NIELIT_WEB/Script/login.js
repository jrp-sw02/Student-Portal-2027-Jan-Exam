// Login Form

$(function() {
    var button = $('#loginButton');
    var box = $('#loginBox');
    var form = $('#loginForm');
    button.removeAttr('href');
    button.mouseup(function(login) {
        box.toggle();
        button.toggleClass('active');
    });
    form.mouseup(function() { 
        return false;
    });
    $(this).mouseup(function(login) {
        if(!($(login.target).parent('#loginButton').length > 0)) {
            button.removeClass('active');
            box.hide();
        }
    });
});

$(function() {
    var button = $('#loginButton2');
    var box = $('#loginBox2');
    var form = $('#loginForm2');
    button.removeAttr('href');
    button.mouseup(function(login) {
        box.toggle();
        button.toggleClass('active');
    });
    form.mouseup(function() { 
        return false;
    });
    $(this).mouseup(function(login) {
        if(!($(login.target).parent('#loginButton2').length > 0)) {
            button.removeClass('active');
            box.hide();
        }
    });
});

//actionContainer
$(function() {
    var actionButton = $('#actionButton');
    var actionBox = $('#actionBox');
    var actionForm = $('#actionhPanel');
    actionButton.removeAttr('href');
    actionButton.mouseup(function (login) {
        actionBox.toggle();
        actionButton.toggleClass('');
    });
    actionForm.mouseup(function () { 
        return false;
    });
    $(this).mouseup(function(login) {
        if (!($(login.target).parent('#actionButton').length > 0)) {
            actionButton.removeClass('active');
            actionBox.hide();
        }
    });
});

$(function() {
    var button = $('#filterButton');
    var box = $('#filterBox');
    var form = $('#filterPannel');
    button.removeAttr('href');
    button.mouseup(function(login) {
        box.toggle();
        button.toggleClass('');
    });
    form.mouseup(function() { 
        return false;
    });
    $(this).mouseup(function(login) {
        if (!($(login.target).parent('#filterButton').length > 0)) {
            button.removeClass('active');
            box.hide();
        }
    });
});