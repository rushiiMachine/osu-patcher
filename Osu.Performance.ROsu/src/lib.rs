use rosu_pp::{Beatmap, OsuPP};
use rosu_pp::osu::{OsuDifficultyAttributes, OsuScoreState};

use crate::structs::{FFIOsuDifficultyAttributes, FFIOsuPerformanceAttributes, FFIOsuScoreState};

mod structs;

#[no_mangle]
unsafe extern "C" fn calculate_osu_performance(
    difficulty: &FFIOsuDifficultyAttributes,
    state: &FFIOsuScoreState,
    performance_out: *mut FFIOsuPerformanceAttributes,
) {
    let difficulty: OsuDifficultyAttributes = difficulty.into();
    let state: OsuScoreState = state.into();

    let performance = OsuPP::new(&Beatmap::default())
        .passed_objects(state.total_hits())
        .attributes(difficulty)
        .state(state)
        .calculate();

    *performance_out = (&performance).into();
}
