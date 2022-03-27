function UpdateExercisesList() ***REMOVED***
    $.ajax(***REMOVED***
        url: "/Exercises/ShowExercisesList",
        type: "GET",
        datatype: "HTML",
        success: function (data) ***REMOVED***
            $('#_CreateExercisePartialForm').removeData('validator');
            $('#_CreateExercisePartialForm').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('#_CreateExercisePartialForm');
            $("#_ShowExercisesPartialList").html(data);
    ***REMOVED***
***REMOVED***)
***REMOVED***
function UpdateCreateExercise() ***REMOVED***
    $.ajax(***REMOVED***
        url: "/Exercises/GetCleanCreateExercisePartial",
        type: "GET",
        datatype: "HTML",
        success: function (data) ***REMOVED***
            $("#_CreateExercisePartialItem").html(data);
    ***REMOVED***
***REMOVED***)
***REMOVED***

function limitExerciseRepetitions(input) ***REMOVED***
    if (Math.abs(input.value) > 0) ***REMOVED***
        input.value = Math.abs(input.value);
***REMOVED*** else ***REMOVED***
        input.value = null;
***REMOVED***
    if (Math.abs(input.value) > 999) ***REMOVED***
        input.value = 999
***REMOVED***
***REMOVED***
function limitExerciseDuration(input) ***REMOVED***
    if (Math.abs(input.value) > 0) ***REMOVED***
        input.value = Math.abs(input.value);
***REMOVED*** else ***REMOVED***
        input.value = null;
***REMOVED***
    if (Math.abs(input.value) > 120) ***REMOVED***
        input.value = 120
***REMOVED***
***REMOVED***