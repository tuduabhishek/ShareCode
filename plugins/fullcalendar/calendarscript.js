var currentUpdateEvent;
var addStartDate;
var addEndDate;
var globalAllDay;

/********** Function to be called when a meeting is updated in DB to update meeting on calendar *********/
function updateEvent(event, element) {
    //alert(event.description);
    
    if ($(this).data("qtip")) $(this).qtip("destroy");

    currentUpdateEvent = event;
    showmodalUpdate();
    // $('#updatedialog').dialog('open');
    $("#eventName").val(event.title);
    $("#eventDesc").val(event.description);
    $("[id*=hdnEventId]").val(event.id);
    $("#eventStart").text("" + event.start.toLocaleString());
    $("#eventEnd").text("" + event.end.toLocaleString());
    $("[id*=txtUpdStartDate]").val(event.Actualstart) ;
    $("[id*=txtUpdEndDate]").val(event.Actualend) ;
    if (event.listAttach.length > 0) {
        $.each(event.listAttach, function (index) {
            if ($(this)[0].attach_type == "MOM") {
                $('[id*=divMoM]').show();
                $('[id*=lblBtnMoM]').html($(this)[0].attach_name);
                $('[id*=hdnMOMID]').val($(this)[0].attach_id);
            } else if ($(this)[0].attach_type == "REVREPO") {
                $('[id*=divReviewReport]').show();
                $('[id*=lblBtnReviewReport]').html($(this)[0].attach_name);
                $('[id*=hdnRevRepoID]').val($(this)[0].attach_id);
            }
        });
    } else {
        $('[id*=divMoM]').hide();
        $('[id*=divReviewReport]').hide();
    }
    if (event.end === null) {
        $("#eventEnd").text("");
    }
    else {
        $("#eventEnd").text("" + event.end.toLocaleString());
    }
    //$('[id*=dummyBtnToBindAttach]').click();

    return false;
}


/********** Function to be called when a meeting is added in DB to add meeting on calendar *********/
function addSuccess(addResult) {
    // if addresult is -1, means event was not added
    //    alert("added key: " + addResult);
    
   
    if (addResult.id != -1) {
        //$('#calendar').fullCalendar('renderEvent',
		//				{
		//				    title: $("#addEventName").val(),
		//				    start: addStartDate,
		//				    end: addEndDate,
		//				    id: addResult,
		//				    description: $("#addEventDesc").val(),
		//				    allDay: globalAllDay,
		//				    backgroundColor: backColor,
		//				    listAttach: attachments
		//				},
		//				true // make the event "stick"
		//			);
        $('#calendar').fullCalendar('renderEvent', addResult, true);// make the event "stick"         

        $('#calendar').fullCalendar('unselect');
    }
}

/********** Function to be called when a cell on calendar control is clicked to add meeting *********/
function selectDate(start, end, allDay) {

    //var check = new Date(start.format('YYYY-MM-DD'));// $.fullCalendar.formatDate(start, 'yyyy-MM-dd');
    //var today = new Date();// $.fullCalendar.formatDate(new Date(), 'yyyy-MM-dd');
    //if (check >= today) {
        // Its a right date
        showmodalAdd();
        $("#addEventStartDate").text("" + start.toLocaleString());
        $("#addEventEndDate").text("" + end.toLocaleString());
        addStartDate = start;
        addEndDate = end;
        globalAllDay = allDay;
   // }
   
   
    //alert(allDay);
}
/********** Function to update when meeting event is drag and dropped *********/
function updateEventOnDropResize(event, allDay) {

    //alert("allday: " + allDay);
    var eventToUpdate = {
        id: event.id,
        start: event.start
    };


    if (event.end === null) {
        eventToUpdate.end = eventToUpdate.start;
    }
    else {
        eventToUpdate.end = event.end;

        
    }

    var endDate;
    if (!event.allDay) {
        endDate = new Date(eventToUpdate.end + 60 * 60000);
        endDate = endDate.toJSON();
    }
    else {
        endDate = eventToUpdate.end.toJSON();
    }

    eventToUpdate.start = eventToUpdate.start.toJSON();
    eventToUpdate.end = eventToUpdate.end.toJSON(); //endDate;
    eventToUpdate.allDay = event.allDay;

    
    $.ajax({
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: '{"eventToUpdate":' + JSON.stringify(eventToUpdate) + '}',
        url: "ManageMeetings.aspx/UpdateEventTime",
        success: function (response) {
            if (response.d) {
                //  GenericMsgModal('success', "updated event with id:" + eventToUpdate.id + " update title to: " + eventToUpdate.title + " update description to: " + eventToUpdate.description);
            } else {
                GenericMsgModal('error', "unable to update event with id:" + eventToUpdate.id + " title : " + eventToUpdate.title + " description : " + eventToUpdate.description);

            }
        },
        error: function (response) {
            //alert("Inside alert");
        }
    });

}
/********** Function to be called when meeting event is drag and dropped *********/
function eventDropped(event, dayDelta, minuteDelta, allDay, revertFunc) {
    if ($(this).data("qtip")) $(this).qtip("destroy");
    updateEventOnDropResize(event);
}
/********** Function to be called when meeting event is resized *********/
function eventResized(event, dayDelta, minuteDelta, revertFunc) {
    if ($(this).data("qtip")) $(this).qtip("destroy");

    updateEventOnDropResize(event);
}
/********** Function to check for any special character in title and description *********/
function checkForSpecialChars(stringToCheck) {
    var pattern = /[^A-Za-z0-9 ]/;
    return pattern.test(stringToCheck);
}
/********** Function to check if meeting is for all day *********/
function isAllDay(startDate, endDate) {
    var allDay;

    if (startDate.format("HH:mm:ss") == "00:00:00" && endDate.format("HH:mm:ss") == "00:00:00") {
        allDay = true;
        globalAllDay = true;
    }
    else {
        allDay = false;
        globalAllDay = false;
    }

    return allDay;
}
/********** Function to show q Tip on hover of any meenting event *********/
function qTipText(start, end, description) {
    var text;

    if (end !== null)
        text = "<strong>Start:</strong> " + start.format("MM/DD/YYYY hh:mm T") + "<br/><strong>End:</strong> " + end.format("MM/DD/YYYY hh:mm T") + "<br/><br/>" + description;
    else
        text = "<strong>Start:</strong> " + start.format("MM/DD/YYYY hh:mm T") + "<br/><strong>End:</strong><br/><br/>" + description;

    return text;
}

$(document).ready(function () {
   
    /********** Update Meeting button click function to update meeting *********/
    $("body").on("click", "[id*=btnUpdateMeeting]", function () {
       
        var spnActualstart = $("[id*=txtUpdStartDate]").val();
        var spnActualend = $("[id*=txtUpdEndDate]").val();
        var momData = $('#MainContent_fuMoM_hdnFile').val() || JSON.stringify({});;
        var reviewReportData = $('#MainContent_fuReviewReport_hdnFile').val() || JSON.stringify({});;
        var updMoMflag = false;
        var updRevRepoflag = false;
        var momID = $('[id*=hdnMOMID]').val();
        var revrepoID = $('[id*=hdnRevRepoID]').val();
        if ($('[id*=divMoM]').is(':visible')) {
            updMoMflag = true

        }
        if ($('[id*=divReviewReport]').is(':visible')) {
            updRevRepoflag = true
        }
        var eventToUpdate = {
            Actualstart: spnActualstart,
            Actualend: spnActualend,
            id: currentUpdateEvent.id,
            title: $("#eventName").val(),
            description: $("#eventDesc").val()
        };



        if (checkForSpecialChars(eventToUpdate.title) || checkForSpecialChars(eventToUpdate.description)) {
            alert("please enter characters: A to Z, a to z, 0 to 9, spaces");
        }
        else {
            
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{"eventToUpdate":' + JSON.stringify(eventToUpdate) + ',"mom":' + momData + ',"rev_repo":' + reviewReportData + ',"org_id":"' + $('[id*=hdnOrgId]').val() + '","user_id":"' + $('[id*=hdnUserID]').val() + '","updMoMFlag":"' + updMoMflag + '","updRevRepoflag":"' + updRevRepoflag + '","momID":"' + momID + '","revrepoID":"' + revrepoID + '"}',
                url: "ManageMeetings.aspx/UpdateEvent",
                success: function (response) {
                    var cevent = JSON.parse(response.d);
                  
                    if (cevent.id == -1) {
                        GenericMsgModal('error', "unable to update event with id:" + eventToUpdate.id + " title : " + eventToUpdate.title + " description : " + eventToUpdate.description);
                    } else {
                        //var mom = JSON.parse(momData) || [];
                        //var revrepo = JSON.parse(reviewReportData) || [];
                        //if (mom) {
                        //    $('[id*=divMoM]').show();
                        //    $('[id*=lnkbtnMoM]').html(mom.name);
                        //    //$('[id*=hdnMOMID]').val(mom.);
                        //}
                        //if (revrepo.length > 0) {
                        //    $('[id*=divReviewReport]').show();
                        //    $('[id*=lnkbtnReviewReport]').html($(this)[0].attach_name);
                        //    $('[id*=hdnRevRepoID]').val($(this)[0].attach_id);
                        //}

                        currentUpdateEvent.title = cevent.title;//$("#eventName").val();
                        currentUpdateEvent.description = cevent.description;//$("#eventDesc").val();
                        currentUpdateEvent.Actualstart = cevent.Actualstart;//spnActualstart;
                        currentUpdateEvent.Actualend = cevent.Actualend;//spnActualend;
                        currentUpdateEvent.listAttach = cevent.listAttach;
                       
                        $('#calendar').fullCalendar('updateEvent', currentUpdateEvent);

                    }
                },
                error: function (response) {
                    //alert("Inside alert");
                }
            });
            //Close Modal
            closeModal('updatedialog');
        }

    });
    /********** Delete Meeting button click function to delete meeting *********/
    $("body").on("click", "[id*=btnDeleteMeeting]", function () {
        swal({
            title: 'Are you sure?',
            text: '',
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            closeOnConfirm: true
        },
           function () {
               var eventId = $("[id*=hdnEventId]").val();
               $.ajax({
                   type: 'POST',
                   contentType: 'application/json; charset=utf-8',
                   data: '{"meetingId":"' + eventId + '"}',
                   url: "ManageMeetings.aspx/DeleteMeeting",
                   success: function (response) {
                       if (response.d) {
                           //  GenericMsgModal('success', 'meeting is deleted');
                           $('#calendar').fullCalendar('removeEvents', eventId);
                       } else {
                           GenericMsgModal('error', 'meeting cannot be deleted now');
                       }

                   },
                   error: function (response) {
                       //alert("Inside alert");
                   }
               });
               //Close Modal
               closeModal('updatedialog');
           });

    });

   
    /********** Add Meeting button click function to create meeting *********/
    $("body").on("click", "[id*=btnAddMeeting]", function () {
       
       
        var spnActualstart = $("[id*=txtActStartDate]").val();
        var spnActualend = $("[id*=txtActEndDate]").val();
        var momData = $('#MainContent_fuNewMoM_hdnFile').val() || JSON.stringify({});
        var reviewReportData = $('#MainContent_fuNewReviewReport_hdnFile').val() || JSON.stringify({});

        var eventToAdd = {
            title: $("#addEventName").val(),
            description: $("#addEventDesc").val(),
            // FullCalendar 1.x
            //start: addStartDate.format("dd-MM-yyyy hh:mm:ss tt"),
            //end: addEndDate.format("dd-MM-yyyy hh:mm:ss tt")

            // FullCalendar 2.x
            start: addStartDate.toJSON(),
            end: addEndDate.toJSON(),
            Actualstart: spnActualstart,
            Actualend: spnActualend,
            allDay: isAllDay(addStartDate, addEndDate)
        };

        if (checkForSpecialChars(eventToAdd.title) || checkForSpecialChars(eventToAdd.description)) {
            GenericMsgModal('warning', "please enter characters: A to Z, a to z, 0 to 9, spaces");
        }
        else {
            
            $.ajax({
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: '{"eventToAdd":' + JSON.stringify(eventToAdd) + ',"org_id":"' + $('[id*=hdnOrgId]').val() + '","user_id":"' + $('[id*=hdnUserID]').val() + '","mom":' + momData + ',"rev_repo":' + reviewReportData + '}',
                url: "ManageMeetings.aspx/CreateMeeting",
                success: function (response) {
                    var cevent = JSON.parse(response.d);
                    if (cevent.id == -1) {
                        GenericMsgModal('error', 'Error in creating meeting');
                    } else {
                        //var attachments = [];
                        //if (JSON.parse(momData).size > 0 || JSON.parse(reviewReportData).size > 0) {
                        //    backColor = '#1BBC9B'; //Success Color
                        //    if (JSON.parse(momData).size > 0) {
                        //        attachments
                        //    }
                        //} else {
                        //    if (new Date(new Date().setDate(new Date(eventToAdd.end).getDate() + 2)) < new Date()) {
                        //        backColor = '#F64747'; //Danger Color
                        //    } else if (new Date(eventToAdd.end) <= new Date(new Date().setDate(new Date().getDate() + 2))) {
                        //        backColor = '#f6bc33'; //Warning Color
                        //    }
                        //}
                        addSuccess(cevent);
                    }
                },
                error: function (response) {
                    //alert("Inside alert");
                }
            });
            //Close Modal
            closeModal('addDialog');
        }
    });
    
  
    //var r = {
    //    left: 'prev,next today',
    //        center: 'title',
    //        right: 'month,agendaWeek,agendaDay'
    //};

  
    /********** Window Resize event  *********/
    $(window).resize(function () {

        //$("#calendar").parents(".control-panel").width() <= 720 ? r = {
        //    month: 'M',
        //    week: 'W',
        //    day: 'D'

        //} : r = {
        //    month: 'Month',
        //    week: 'Week',
        //    day: 'Day'
        //};
        // $('#calendar').fullCalendar('option', 'buttonText', get_button_text());
        $('#calendar').fullCalendar('option', 'height', get_calendar_height());
        //$("#calendar").fullCalendar('refresh');
        //$('#calendar').fullCalendar({
        //    buttonText: r
        //});
    });

   
    var options = {
        weekday: "long", year: "numeric", month: "short",
        day: "numeric", hour: "2-digit", minute: "2-digit"
    };
    /********** Month button click on Months Tab to change month *********/
    $('body').on('click', '.Month', function () {
        var ul = $('.nav');
        var li = $(this).closest('li');
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();
        var year =  null;
        if ($(this)[0].id < 4) {
            if (m < 3) {
                year = y;
            } else {
                year = y + 1;
            }
        } else {
            if (m < 3) {
                year = y - 1;
            } else {
                year = y;
            }
            
        }
        var clickedDat = new Date(year, $(this)[0].id - 1, d);
        /// alert(clickedDat);
        ul.find('li.active').removeClass('active');
        li.addClass('active');
        $('#calendar').fullCalendar('gotoDate', clickedDat);
    });
    /********** Initialise Calendar Control *********/
   // InitCalendar();

});
function activeTab() {
    var ul = $('.nav');
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth() + 1 ;
    if (m < 10) {
        m = '0' + m;
    }
    var y = date.getFullYear();

    ul.find('li [id*=' + m  + ']').parent().addClass('active');
}
/**********Function to Initialise Calendaer Control for View Only******************/
function InitCalendarViewOnly() {

    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        eventLimit: true,
        views: {
            month: {
                eventLimit: 4 // adjust to 3 only for month
            },
            week: {
                eventLimit: 3// options apply to basicWeek and agendaWeek views
            },
            day: {
                eventLimit: 2 // options apply to basicDay and agendaDay views
            }
        },
        height: get_calendar_height,
        selectable: false,
        editable: false,
        events: "EventHandler.ashx",
        eventRender: function (event, element) {
            //alert(event.title);
            element.qtip({
                content: {
                    text: qTipText(event.start, event.end, event.description),
                    title: '<strong>' + event.title + '</strong>',
                    button: true
                },
                position: {
                    viewport: $(window)
                    //my: 'top left',
                    //at: 'bottom left'
                },
                style: {
                    classes: 'qtip-shadow qtip-rounded'

                }

            });
        }
    });
}
/**********Function to Initialise Calendar Control *********/
function InitCalendar() {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        eventLimit: true,
        views: {
            month: {
                eventLimit: 4 // adjust to 3 only for month
            },
            week: {
                eventLimit: 3// options apply to basicWeek and agendaWeek views
            },
            day: {
                eventLimit: 2 // options apply to basicDay and agendaDay views
            }
        },
        height: get_calendar_height,
        //buttonText: get_button_text,
        eventClick: updateEvent,
        selectable: true,
        selectHelper: true,
        select: selectDate,
        editable: true,
        events: "EventHandler.ashx",
        eventDrop: eventDropped,
        eventResize: eventResized,
        eventRender: function (event, element) {
            //alert(event.title);
            element.qtip({
                content: {
                    text: qTipText(event.start, event.end, event.description),
                    title: '<strong>' + event.title + '</strong>',
                    button: true
                },
                position: {
                    viewport: $(window)
                    //my: 'top left',
                    //at: 'bottom left'
                },
                style: {
                    classes: 'qtip-shadow qtip-rounded'

                }

            });
        }
    });
}
/********** function to calculate window height  *********/
function get_calendar_height() {
    return $(window).height() - 30;
}
/********** function to get button text on resize  *********/
function get_button_text() {
    var r = {};

    return r;
}
/********** Function to show generic message modal *********/
function GenericMsgModal(type, msg) {
    swal('', msg, type);
}
/********** Function to show update meeting dialog *********/
function showmodalUpdate() {
    $('#updatedialog').modal({
        'show': true
    });
}
/********** Function to show add meeting dialog *********/
function showmodalAdd() {
    $('#addDialog').modal({
        'show': true
    });
}
/********** Function to close any dialog *********/
function closeAllModal() {
    $(".modal").modal("hide");
    $('body').removeClass('modal-open');
    $('.modal-backdrop').remove();
};
/********** Function to close particular dialog *********/
function closeModal(id) {
    $('#' + id).modal("hide");
    //$('body').removeClass('modal-open');
    // $('.modal-backdrop').remove();
};
/********** Function to show error dialog *********/
function showmodalError() {
    $('#errordialog').modal({
        'show': true
    });
}