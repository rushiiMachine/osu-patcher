use std::ffi::{c_char, CStr};
use std::fs::File;

use rosu_pp::{Beatmap, OsuPP};
use rosu_pp::osu::{OsuDifficultyAttributes, OsuOwnedGradualPerformance, OsuScoreState};

use crate::structs::{FFIOsuDifficultyAttributes, FFIOsuPerformanceAttributes, FFIOsuScoreState, OsuJudgementResult};

mod structs;

#[no_mangle]
extern "C" fn calculate_osu_performance(
    difficulty: &FFIOsuDifficultyAttributes,
    state: &FFIOsuScoreState,
    mods: u32,
    performance_out: &mut FFIOsuPerformanceAttributes,
) {
    let difficulty: OsuDifficultyAttributes = difficulty.into();
    let state: OsuScoreState = state.into();

    let performance = OsuPP::new(&Beatmap::default())
        .mods(mods)
        .passed_objects(state.total_hits())
        .attributes(difficulty)
        .state(state)
        .calculate();

    *performance_out = (&performance).into();
}

#[no_mangle]
extern "C" fn initialize_osu_performance_gradual(
    map_path: *const c_char,
    mods: u32,
) -> *mut OsuGradualPerformanceState {
    let map_path_bytes = unsafe { CStr::from_ptr(map_path) }.to_bytes();
    let map_path: &str = unsafe { std::str::from_utf8_unchecked(map_path_bytes) };

    let map = Beatmap::parse(File::open(map_path).unwrap()).unwrap(); // TODO: handle errors
    let performance = OsuOwnedGradualPerformance::new(map, mods);
    let state = OsuGradualPerformanceState {
        performance,
        score: OsuScoreState::default(),
    };

    Box::into_raw(Box::new(state))
}

#[no_mangle]
extern "C" fn calculate_osu_performance_gradual(
    state: &mut OsuGradualPerformanceState,
    new_judgement: OsuJudgementResult,
    max_combo: u64,
) -> f64 {
    state.score.max_combo = max_combo as usize;

    match new_judgement {
        OsuJudgementResult::Result300 => state.score.n300 += 1,
        OsuJudgementResult::Result100 => state.score.n100 += 1,
        OsuJudgementResult::Result50 => state.score.n50 += 1,
        OsuJudgementResult::ResultMiss => state.score.n_misses += 1,
    }

    let performance = state.performance.next(state.score.clone())
        .unwrap(); // TODO: handle errors

    return performance.pp;
}

#[no_mangle]
extern "C" fn dispose_osu_performance_gradual(
    state: *mut OsuGradualPerformanceState,
) {
    drop(unsafe { Box::from_raw(state) });
}

struct OsuGradualPerformanceState {
    performance: OsuOwnedGradualPerformance,
    score: OsuScoreState,
}
