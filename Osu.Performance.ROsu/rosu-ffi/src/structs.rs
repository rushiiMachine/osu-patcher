use rosu_pp::osu::{OsuDifficultyAttributes, OsuPerformanceAttributes, OsuScoreState};

#[repr(C)]
pub struct FFIOsuDifficultyAttributes {
    pub stars: f64,
    pub max_combo: u64,
    pub speed_note_count: f64,

    pub approach_rate: f64,
    pub overall_difficulty: f64,
    pub health_rate: f64,

    pub aim_skill: f64,
    pub speed_skill: f64,
    pub flashlight_skill: f64,
    pub slider_skill: f64,

    pub circle_count: u64,
    pub slider_count: u64,
    pub spinner_count: u64,
}

#[repr(C)]
pub struct FFIOsuScoreState {
    pub max_combo: u64,
    pub count_300: u64,
    pub count_100: u64,
    pub count_50: u64,
    pub count_0: u64,
}

#[repr(C)]
pub struct FFIOsuPerformanceAttributes {
    pub pp_total: f64,
    pub pp_aim: f64,
    pub pp_speed: f64,
    pub pp_accuracy: f64,
    pub pp_flashlight: f64,
    pub effective_miss_count: f64,
}

#[repr(u8)]
pub enum OsuJudgementResult {
    Result300 = 1,
    Result100 = 2,
    Result50 = 3,
    ResultMiss = 4,
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
    #[inline]
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
    #[inline]
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
