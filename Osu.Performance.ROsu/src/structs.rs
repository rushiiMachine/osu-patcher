use rosu_pp::osu::{OsuDifficultyAttributes, OsuPerformanceAttributes, OsuScoreState};

#[repr(C)]
pub struct FFIOsuDifficultyAttributes {
    stars: f64,
    max_combo: u64,
    speed_note_count: f64,

    approach_rate: f64,
    overall_difficulty: f64,
    health_rate: f64,

    aim_skill: f64,
    speed_skill: f64,
    flashlight_skill: f64,
    slider_skill: f64,

    circle_count: u64,
    slider_count: u64,
    spinner_count: u64,
}

#[repr(C)]
pub struct FFIOsuScoreState {
    max_combo: u64,
    count_300: u64,
    count_100: u64,
    count_50: u64,
    count_0: u64,
}

#[repr(C)]
pub struct FFIOsuPerformanceAttributes {
    pp_total: f64,
    pp_aim: f64,
    pp_speed: f64,
    pp_accuracy: f64,
    pp_flashlight: f64,
    effective_miss_count: f64,
}

impl From<&FFIOsuDifficultyAttributes> for OsuDifficultyAttributes {
    #[inline]
    fn from(value: &FFIOsuDifficultyAttributes) -> Self {
        OsuDifficultyAttributes {
            stars: value.stars,
            max_combo: value.max_combo as usize,
            speed_note_count: value.speed_note_count,

            ar: value.approach_rate,
            od: value.overall_difficulty,
            hp: value.health_rate,

            aim: value.aim_skill,
            speed: value.speed_skill,
            flashlight: value.flashlight_skill,
            slider_factor: value.slider_skill,

            n_circles: value.circle_count as usize,
            n_sliders: value.slider_count as usize,
            n_spinners: value.spinner_count as usize,
        }
    }
}

impl From<&FFIOsuScoreState> for OsuScoreState {
    fn from(value: &FFIOsuScoreState) -> Self {
        OsuScoreState {
            max_combo: value.max_combo as usize,
            n300: value.count_300 as usize,
            n100: value.count_100 as usize,
            n50: value.count_50 as usize,
            n_misses: value.count_0 as usize,
        }
    }
}

impl From<&OsuPerformanceAttributes> for FFIOsuPerformanceAttributes {
    fn from(value: &OsuPerformanceAttributes) -> Self {
        FFIOsuPerformanceAttributes {
            pp_total: value.pp,
            pp_aim: value.pp_aim,
            pp_speed: value.pp_speed,
            pp_accuracy: value.pp_acc,
            pp_flashlight: value.pp_flashlight,
            effective_miss_count: value.effective_miss_count,
        }
    }
}
