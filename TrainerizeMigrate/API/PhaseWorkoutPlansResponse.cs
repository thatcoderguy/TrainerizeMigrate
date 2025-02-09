using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainerizeMigrate.API
{
    public class WorkoutAudioUrl
    {
        public string? male { get; set; }
        public string? female { get; set; }
    }

    public class CreatedBy
    {
        public int? id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }

    public class PhaseWorkoutPlanDefault
    {
        public string? videoToken { get; set; }
        public string? loopVideoToken { get; set; }
        public WorkoutVideoUrls? videoUrls { get; set; }
        public WorkoutLoopVideoUrls? loopVideoUrls { get; set; }
        public WorkoutThumbnailUrls? thumbnailUrls { get; set; }
    }

    public class PhaseWorkoutPlanExercise
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int? sets { get; set; }
        public string? target { get; set; }
        public TargetDetail? targetDetail { get; set; }
        public string? side { get; set; }
        public int? superSetID { get; set; }
        public string? supersetType { get; set; }
        public int? intervalTime { get; set; }
        public int? restTime { get; set; }
        public string? recordType { get; set; }
        public string? version { get; set; }
        public WorkoutMedia? media { get; set; }
        public string? videoType { get; set; }
        public string? videoUrl { get; set; }
        public WorkoutVideoMobileUrl? videoMobileUrl { get; set; }
        public string? videoStatus { get; set; }
    }

    public class WorkoutFemale
    {
        public string? videoToken { get; set; }
        public string? loopVideoToken { get; set; }
        public WorkoutVideoUrls? videoUrls { get; set; }
        public WorkoutLoopVideoUrls? loopVideoUrls { get; set; }
        public WorkoutThumbnailUrls? thumbnailUrls { get; set; }
    }

    public class WorkoutLoopVideoUrl
    {
        public string? sd { get; set; }
        public string? hd { get; set; }
        public string? fhd { get; set; }
        public string? hls { get; set; }
        public string? hlssd { get; set; }
        public string? hlshd { get; set; }
    }

    public class WorkoutLoopVideoUrls
    {
        public string? sd { get; set; }
        public string? hd { get; set; }
        public string? fhd { get; set; }
        public string? hls { get; set; }
        public string? hlssd { get; set; }
        public string? hlshd { get; set; }
    }

    public class WorkoutMedia
    {
        public bool? isOverride { get; set; }
        public string? type { get; set; }
        public string? status { get; set; }
        public string? token { get; set; }
        public string? loopVideoToken { get; set; }
        public WorkoutVideoUrl? videoUrl { get; set; }
        public WorkoutLoopVideoUrl? loopVideoUrl { get; set; }
        public WorkoutThumbnailUrl? thumbnailUrl { get; set; }
        public WorkoutAudioUrl? audioUrl { get; set; }
        public PhaseWorkoutPlanDefault? @default { get; set; }
        public WorkoutFemale? female { get; set; }
    }

    public class PhaseWorkoutPlansResponse
    {
        public int? total { get; set; }
        public List<Workout>? workouts { get; set; }
    }

    public class TargetDetail
    {
        public int type { get; set; }
        public int? distance { get; set; }
        public string? distanceUnit { get; set; }
        public double? time { get; set; }
        public string? text { get; set; }
        public string? zone { get; set; }
    }

    public class WorkoutThumbnailUrl
    {
        public string? hd { get; set; }
        public string? sd { get; set; }
    }

    public class WorkoutThumbnailUrls
    {
        public string? hd { get; set; }
        public string? sd { get; set; }
    }

    public class WorkoutVideoMobileUrl
    {
        public string? mobile { get; set; }
        public string? hd { get; set; }
        public string? fhd { get; set; }
        public string? hls { get; set; }
        public string? hlssd { get; set; }
        public string? hlshd { get; set; }
    }

    public class WorkoutVideoUrl
    {
        public string? sd { get; set; }
        public string? hd { get; set; }
        public string? fhd { get; set; }
        public string? hls { get; set; }
        public string? hlssd { get; set; }
        public string? hlshd { get; set; }
    }

    public class WorkoutVideoUrls
    {
        public string? sd { get; set; }
        public string? hd { get; set; }
        public string? fhd { get; set; }
        public string? hls { get; set; }
        public string? hlssd { get; set; }
        public string? hlshd { get; set; }
    }

    public class Workout
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int? duration { get; set; }
        public string? instruction { get; set; }
        public string? type { get; set; }
        public Media? media { get; set; }
        public List<PhaseWorkoutPlanExercise>? exercises { get; set; }
        public bool? fromHQ { get; set; }
        public int? workoutSource { get; set; }
        public string? dateCreated { get; set; }
        public string? dateUpdated { get; set; }
        public string? accessLevel { get; set; }
        public CreatedBy? createdBy { get; set; }
        public List<object>? tags { get; set; }
    }


}
