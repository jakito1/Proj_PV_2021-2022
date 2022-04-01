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
function UpdateMealsList() ***REMOVED***
    $.ajax(***REMOVED***
        url: "/Meals/ShowMealsList",
        type: "GET",
        datatype: "HTML",
        success: function (data) ***REMOVED***
            $('#_CreateMealPartialForm').removeData('validator');
            $('#_CreateMealPartialForm').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('#_CreateMealPartialForm');
            $("#_ShowMealsPartialList").html(data);
    ***REMOVED***
***REMOVED***)
***REMOVED***
function UpdateCreateMeal() ***REMOVED***
    $.ajax(***REMOVED***
        url: "/Meals/GetCleanCreateMealPartial",
        type: "GET",
        datatype: "HTML",
        success: function (data) ***REMOVED***
            $("#_CreateMealPartialItem").html(data);
    ***REMOVED***
***REMOVED***)
***REMOVED***

function showDateOrWeekDayDiv(select) ***REMOVED***
    if (select.value == 1) ***REMOVED***
        document.getElementById('MealDateID').style.display = "block";
        document.getElementById('MealWeekDayID').style.display = "none";
        document.getElementById('MealWeekDaySelectID').value = ''
***REMOVED*** else ***REMOVED***
        document.getElementById('MealDateID').style.display = "none";
        document.getElementById('MealWeekDayID').style.display = "block";
        document.getElementById('MealDateInputID').value = ''
***REMOVED***
***REMOVED*** 

function hideButtonShowCreateTrainingPlanEditRequestPartial() ***REMOVED***
    document.getElementById('planEditButton').style.display = "none";
    document.getElementById('_CreateTrainingPlanEditRequestPartialItem').style.display = "block";
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
function limitMealForm(input) ***REMOVED***
    if (Math.abs(input.value) > 0) ***REMOVED***
        input.value = Math.abs(input.value);
***REMOVED*** else ***REMOVED***
        input.value = null;
***REMOVED***
    if (Math.abs(input.value) > 99999) ***REMOVED***
        input.value = 99999
***REMOVED***
***REMOVED***
