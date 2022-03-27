function UpdateExercisesList() {
    $.ajax({
        url: "/Exercises/ShowExercisesList",
        type: "GET",
        datatype: "HTML",
        success: function (data) {
            $('#_CreateExercisePartialForm').removeData('validator');
            $('#_CreateExercisePartialForm').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('#_CreateExercisePartialForm');
            $("#_ShowExercisesPartialList").html(data);
        }
    })
}
function UpdateCreateExercise() {
    $.ajax({
        url: "/Exercises/GetCleanCreateExercisePartial",
        type: "GET",
        datatype: "HTML",
        success: function (data) {
            $("#_CreateExercisePartialItem").html(data);
        }
    })
}

function limitExerciseRepetitions(input) {
    if (Math.abs(input.value) > 0) {
        input.value = Math.abs(input.value);
    } else {
        input.value = null;
    }
    if (Math.abs(input.value) > 999) {
        input.value = 999
    }
}
function limitExerciseDuration(input) {
    if (Math.abs(input.value) > 0) {
        input.value = Math.abs(input.value);
    } else {
        input.value = null;
    }
    if (Math.abs(input.value) > 120) {
        input.value = 120
    }
}