use rosu_pp::{Beatmap, OsuPP};
use rosu_pp::osu::OsuDifficultyAttributes;

#[repr(C)]
struct _OsuDifficultyAttributes {
    aim: f64,
    speed: f64,
    flashlight: f64,
    slider_factor: f64,
    speed_note_count: f64,
    ar: f64,
    od: f64,
    hp: f64,
    circles: usize,
    sliders: usize,
    spinners: usize,
    stars: f64,
    max_combo: usize,
}

impl From<&_OsuDifficultyAttributes> for OsuDifficultyAttributes {
    fn from(value: &_OsuDifficultyAttributes) -> Self {
        OsuDifficultyAttributes {
            aim: value.aim,
            speed: value.speed,
            flashlight: value.flashlight,
            slider_factor: value.slider_factor,
            speed_note_count: value.speed_note_count,
            ar: value.ar,
            od: value.od,
            hp: value.hp,
            n_circles: value.circles,
            n_sliders: value.sliders,
            n_spinners: value.spinners,
            stars: value.stars,
            max_combo: value.max_combo,
        }
    }
}

#[no_mangle]
extern "C" fn test() -> i32 {
    727
}

#[no_mangle]
extern "C" fn calculate_osu(difficulty: &_OsuDifficultyAttributes) -> f64 {
    let diff: OsuDifficultyAttributes = difficulty.into();

    return OsuPP::new(&Beatmap::default())
        // https://github.com/MaxOhn/rosu-pp/issues/28#issuecomment-1871814571
        .passed_objects(diff.n_circles + diff.n_sliders + diff.n_spinners)
        .attributes(diff)
        .calculate()
        .pp();
}
