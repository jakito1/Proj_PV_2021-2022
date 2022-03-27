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