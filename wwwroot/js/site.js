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
function UpdateMealsList() {
    $.ajax({
        url: "/Meals/ShowMealsList",
        type: "GET",
        datatype: "HTML",
        success: function (data) {
            $('#_CreateMealPartialForm').removeData('validator');
            $('#_CreateMealPartialForm').removeData('unobtrusiveValidation');
            $.validator.unobtrusive.parse('#_CreateMealPartialForm');
            $("#_ShowMealsPartialList").html(data);
        }
    })
}
function UpdateCreateMeal() {
    $.ajax({
        url: "/Meals/GetCleanCreateMealPartial",
        type: "GET",
        datatype: "HTML",
        success: function (data) {
            $("#_CreateMealPartialItem").html(data);
        }
    })
}

function showDateOrWeekDayDiv(select) {
    if (select.value == 1) {
        document.getElementById('MealDateID').style.display = "block";
        document.getElementById('MealWeekDayID').style.display = "none";
        document.getElementById('MealWeekDaySelectID').value = ''
    } else {
        document.getElementById('MealDateID').style.display = "none";
        document.getElementById('MealWeekDayID').style.display = "block";
        document.getElementById('MealDateInputID').value = ''
    }
} 

function hideButtonPlanEditRequestPartial() {
    document.getElementById('planEditButton').style.display = "none";
    document.getElementById('_CreateEditRequestPartialItem').style.display = "block";
}

function helpButton() {
    if (document.getElementById('helpText').style.display == "none") {
        document.getElementById('helpText').style.display = "block";
    } else {
        document.getElementById('helpText').style.display = "none";
    }
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
function limitMealForm(input) {
    if (Math.abs(input.value) > 0) {
        input.value = Math.abs(input.value);
    } else {
        input.value = null;
    }
    if (Math.abs(input.value) > 99999) {
        input.value = 99999
    }
}
function limitPercentage(input) {
    if (Math.abs(input.value) > 0) {
        input.value = Math.abs(input.value);
    } else {
        input.value = null;
    }
    if (Math.abs(input.value) > 100) {
        input.value = 100
    }
}

window.addEventListener("load", () => {
    const uri = document.getElementById("qrCodeData").getAttribute('data-url');
    new QRCode(document.getElementById("qrCode"),
        {
            text: uri,
            width: 150,
            height: 150
        });
});
