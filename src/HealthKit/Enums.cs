using CoreFoundation;
using ObjCRuntime;
using Foundation;
using System;

namespace HealthKit {
	// NSInteger -> HKDefines.h
	/// <summary>Enumerates the frequences for background delivery of data (see <see cref="M:HealthKit.HKHealthStore.EnableBackgroundDelivery(HealthKit.HKObjectType,HealthKit.HKUpdateFrequency,System.Action{System.Boolean,Foundation.NSError})" />).</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKUpdateFrequency : long {
		Immediate = 1,
		Hourly,
		Daily,
		Weekly,
	}

	// NSInteger -> HKDefines.h
	/// <summary>Enumerates the permission of the app to read or write health data.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKAuthorizationStatus : long {
		/// <summary>The permission has not yet been determined.</summary>
		NotDetermined = 0,
		/// <summary>Sharing is not allowed.</summary>
		SharingDenied,
		/// <summary>Sharing is allowed.</summary>
		SharingAuthorized,
	}

	// NSInteger -> HKDefines.h
	/// <summary>Enumerates the biological sexes.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKBiologicalSex : long {
		/// <summary>The user has not recorded their sex in the gender binary.</summary>
		NotSet = 0,
		/// <summary>The female sex.</summary>
		Female,
		/// <summary>The male sex.</summary>
		Male,
		/// <summary>The user is not categorized as either male or female.</summary>
		[MacCatalyst (13, 1)]
		Other,
	}

	// NSInteger -> HKDefines.h
	/// <summary>Enumerates known blood types.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKBloodType : long {
		/// <summary>The user's blood type is not recorded.</summary>
		NotSet = 0,
		/// <summary>The A+ blood type.</summary>
		APositive,
		/// <summary>The A- blood type.</summary>
		ANegative,
		/// <summary>The B+ blood type.</summary>
		BPositive,
		/// <summary>The B- blood type.</summary>
		BNegative,
		/// <summary>The AB+ blood type</summary>
		ABPositive,
		/// <summary>The AB- blood type.</summary>
		ABNegative,
		/// <summary>The O+ blood type.</summary>
		OPositive,
		/// <summary>The O- blood type.</summary>
		ONegative,
	}

	// NSInteger -> HKMetadata.h
	/// <summary>Enumerates the positions at which a thermometer takes its reading.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKBodyTemperatureSensorLocation : long {
		/// <summary>The temperature reading location is unspecified.</summary>
		Other = 0,
		/// <summary>The temperature reading is taken at the user's armpit.</summary>
		Armpit,
		/// <summary>The temperature reading is taken somewhere on the user's body.</summary>
		Body,
		/// <summary>The temperature reading is taken at the user's ear.</summary>
		Ear,
		/// <summary>The temperature reading is taken at the user's finger.</summary>
		Finger,
		/// <summary>The temperature reading is taken in the user's gastrointestinal system.</summary>
		GastroIntestinal,
		/// <summary>The temperature reading is taken in the user's mouth.</summary>
		Mouth,
		/// <summary>The temperature reading is taken at the user's rectum.</summary>
		Rectum,
		/// <summary>The temperature reading is taken at the user's toe.</summary>
		Toe,
		/// <summary>The temperature reading is taken at the user's eardrum.</summary>
		EarDrum,
		/// <summary>The temperature reading is taken at the user's temporal artery.</summary>
		TemporalArtery,
		/// <summary>The temperature reading is taken at the user's forehead.</summary>
		Forehead,
	}

	// NSInteger -> HKMetadata.h
	/// <summary>Enumerates the locations at which a heart rate monitor is attached.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKHeartRateSensorLocation : long {
		Other = 0,
		Chest,
		Wrist,
		Finger,
		Hand,
		EarLobe,
		Foot,
	}

	// NSInteger -> HKObjectType.h
	/// <summary>Enumerates whether an <see cref="T:HealthKit.HKQuantityType" /> is a cumulative measure (for instance, "active energy burned") or a discrete value (such as "blood alcohol content").</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKQuantityAggregationStyle : long {
		Cumulative = 0,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		DiscreteArithmetic,
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'HKQuantityAggregationStyle.DiscreteArithmetic'.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'HKQuantityAggregationStyle.DiscreteArithmetic'.")]
		Discrete = DiscreteArithmetic,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		DiscreteTemporallyWeighted,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		DiscreteEquivalentContinuousLevel,
	}

	// NSInteger -> HKObjectType.h
	/// <summary>Enumerates the states of the slumberer: whether they are asleep or merely resting in bed.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueSleepAnalysis : long {
		/// <summary>The user is in bed.</summary>
		InBed,
		/// <summary>The user is asleep.</summary>
		Asleep,
		/// <summary>To be added.</summary>
		[MacCatalyst (13, 1)]
		Awake,
		[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
		AsleepCore = 3,
		[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
		AsleepDeep = 4,
		[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
		AsleepREM = 5,
	}

	// NSUInteger -> HKQuery.h
	/// <summary>Enumerates options available for use with the <see cref="M:HealthKit.HKQuery.GetPredicateForSamples(Foundation.NSDate,Foundation.NSDate,HealthKit.HKQueryOptions)" /> method.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum HKQueryOptions : ulong {
		None = 0,
		StrictStartDate = 1 << 0,
		StrictEndDate = 1 << 1,
	}

	// NSUInteger -> HKStatistics.h
	/// <summary>Enumerates options applicable to <see cref="T:HealthKit.HKStatisticsQuery" /> and <see cref="T:HealthKit.HKStatisticsCollectionQuery" /> objets.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	[Flags]
	public enum HKStatisticsOptions : ulong {
		None = 0,
		SeparateBySource = 1 << 0,
		DiscreteAverage = 1 << 1,
		DiscreteMin = 1 << 2,
		DiscreteMax = 1 << 3,
		CumulativeSum = 1 << 4,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		MostRecent = 1 << 5,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Duration = 1 << 6,
	}

	// NSInteger -> HKUnit.h
	/// <summary>Enumerates metric prefixes, e.g., Centi-, Deca-, Deci-. Used with factory methods of <see cref="T:HealthKit.HKUnit" />.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKMetricPrefix : long {
		None = 0,
		Pico,
		Nano,
		Micro,
		Milli,
		Centi,
		Deci,
		Deca,
		Hecto,
		Kilo,
		Mega,
		Giga,
		Tera,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		Femto,
	}

	/// <summary>Enumerates various activities that are considered workouts.</summary>
	[Native]
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	public enum HKWorkoutActivityType : ulong {
		AmericanFootball = 1,
		Archery,
		AustralianFootball,
		Badminton,
		Baseball,
		Basketball,
		Bowling,
		Boxing,
		Climbing,
		Cricket,
		CrossTraining,
		Curling,
		Cycling,
		Dance,
		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'HKWorkoutActivityType.Dance', 'HKWorkoutActivityType.Barre', or 'HKWorkoutActivityType.Pilates'.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'HKWorkoutActivityType.Dance', 'HKWorkoutActivityType.Barre', or 'HKWorkoutActivityType.Pilates'.")]
		DanceInspiredTraining,
		Elliptical,
		EquestrianSports,
		Fencing,
		Fishing,
		FunctionalStrengthTraining,
		Golf,
		Gymnastics,
		Handball,
		Hiking,
		Hockey,
		Hunting,
		Lacrosse,
		MartialArts,
		MindAndBody,
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'MixedCardio' or 'HighIntensityIntervalTraining' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'MixedCardio' or 'HighIntensityIntervalTraining' instead.")]
		MixedMetabolicCardioTraining,
		PaddleSports,
		Play,
		PreparationAndRecovery,
		Racquetball,
		Rowing,
		Rugby,
		Running,
		Sailing,
		SkatingSports,
		SnowSports,
		Soccer,
		Softball,
		Squash,
		StairClimbing,
		SurfingSports,
		Swimming,
		TableTennis,
		Tennis,
		TrackAndField,
		TraditionalStrengthTraining,
		Volleyball,
		Walking,
		WaterFitness,
		WaterPolo,
		WaterSports,
		Wrestling,
		Yoga,
		[MacCatalyst (13, 1)]
		Barre,
		[MacCatalyst (13, 1)]
		CoreTraining,
		[MacCatalyst (13, 1)]
		CrossCountrySkiing,
		[MacCatalyst (13, 1)]
		DownhillSkiing,
		[MacCatalyst (13, 1)]
		Flexibility,
		[MacCatalyst (13, 1)]
		HighIntensityIntervalTraining,
		[MacCatalyst (13, 1)]
		JumpRope,
		[MacCatalyst (13, 1)]
		Kickboxing,
		[MacCatalyst (13, 1)]
		Pilates,
		[MacCatalyst (13, 1)]
		Snowboarding,
		[MacCatalyst (13, 1)]
		Stairs,
		[MacCatalyst (13, 1)]
		StepTraining,
		[MacCatalyst (13, 1)]
		WheelchairWalkPace,
		[MacCatalyst (13, 1)]
		WheelchairRunPace,
		[MacCatalyst (13, 1)]
		TaiChi,
		[MacCatalyst (13, 1)]
		MixedCardio,
		[MacCatalyst (13, 1)]
		HandCycling,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		DiscSports,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		FitnessGaming,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		CardioDance = 77,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		SocialDance = 78,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Pickleball = 79,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		Cooldown = 80,
		[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
		SwimBikeRun = 82,
		[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
		Transition = 83,
		[MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0), NoTV]
		UnderwaterDiving,
		[MacCatalyst (13, 1)]
		Other = 3000,
	}

	/// <summary>Enumerates events that can occur during a workout (Pause, Resume).</summary>
	[Native]
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	public enum HKWorkoutEventType : long {
		Pause = 1,
		Resume,
		[MacCatalyst (13, 1)]
		Lap,
		[MacCatalyst (13, 1)]
		Marker,
		[MacCatalyst (13, 1)]
		MotionPaused,
		[MacCatalyst (13, 1)]
		MotionResumed,
		[MacCatalyst (13, 1)]
		Segment,
		[MacCatalyst (13, 1)]
		PauseOrResumeRequest,
	}

	/// <summary>Contains a single value that indicates that a category value is not applicable to the category.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValue : long {
		/// <summary>A category value is not applicable to the category.</summary>
		NotApplicable = 0,
	}

	/// <summary>Enumerates the user's cervical mucus quality.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueCervicalMucusQuality : long {
		/// <summary>Either not recorded or permissions not given.</summary>
		NotApplicable = 0,
		/// <summary>Little or no mucus.</summary>
		Dry = 1,
		/// <summary>Sticky mucus.</summary>
		Sticky,
		/// <summary>Creamy mucus.</summary>
		Creamy,
		/// <summary>Watery mucus.</summary>
		Watery,
		/// <summary>Mucus the color and consistency of egg whites.</summary>
		EggWhite,
	}

	/// <summary>Enumerates the amount of menstrual flow.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	[Deprecated (PlatformName.iOS, 18, 0, message: "Use 'HKCategoryValueVaginalBleeding' instead.")]
	[Deprecated (PlatformName.MacCatalyst, 18, 0, message: "Use 'HKCategoryValueVaginalBleeding' instead.")]
	[Deprecated (PlatformName.TvOS, 18, 0, message: "Use 'HKCategoryValueVaginalBleeding' instead.")]
	[Deprecated (PlatformName.MacOSX, 15, 0, message: "Use 'HKCategoryValueVaginalBleeding' instead.")]
	public enum HKCategoryValueMenstrualFlow : long {
		/// <summary>Categorization is not applicable.</summary>
		NotApplicable = 0,
		/// <summary>The level is unspecified.</summary>
		Unspecified = 1,
		/// <summary>Light flow.</summary>
		Light,
		/// <summary>Medium flow.</summary>
		Medium,
		/// <summary>Heavy flow.</summary>
		Heavy,
		[MacCatalyst (13, 1)]
		None,
	}

	[TV (18, 0), Mac (15, 0), iOS (18, 0), MacCatalyst (18, 0)]
	[Native]
	public enum HKCategoryValueVaginalBleeding : long {
		Unspecified = 1,
		Light = 2,
		Medium = 3,
		Heavy = 4,
		None = 5,
	}

	/// <summary>Enumerates the results of an ovulation test.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueOvulationTestResult : long {
		/// <summary>The test is not applicable.</summary>
		NotApplicable = 0,
		/// <summary>The test was negative.</summary>
		Negative = 1,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		LuteinizingHormoneSurge = 2,
		/// <summary>The test was positive.</summary>
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use 'HKCategoryValueOvulationTestResult.LuteinizingHormoneSurge' instead.")]
		[Deprecated (PlatformName.MacCatalyst, 13, 1, message: "Use 'HKCategoryValueOvulationTestResult.LuteinizingHormoneSurge' instead.")]
		Positive = LuteinizingHormoneSurge,
		/// <summary>The test was indeterminate.</summary>
		Indeterminate = 3,
		[iOS (13, 0)]
		[MacCatalyst (13, 1)]
		EstrogenSurge = 4,
	}

	/// <summary>Enumerates whether the user stood or not during an hour.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueAppleStandHour : long {
		/// <summary>The user did stand for at least a few minutes.</summary>
		Stood = 0,
		/// <summary>The user did not stand.</summary>
		Idle,
	}

	[iOS (13, 0)]
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueAudioExposureEvent : long {
		LoudEnvironment = 1,
	}

	/// <summary>Enumerates skin types using the Fitzpatrick scale.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKFitzpatrickSkinType : long {
		NotSet = 0,
		I,
		II,
		III,
		IV,
		V,
		VI,
	}

	/// <summary>Enumerates constants that describe wheelchair use.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKWheelchairUse : long {
		NotSet = 0,
		No,
		Yes,
	}

	/// <summary>Enumerates weather types.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKWeatherCondition : long {
		None = 0,
		Clear,
		Fair,
		PartlyCloudy,
		MostlyCloudy,
		Cloudy,
		Foggy,
		Haze,
		Windy,
		Blustery,
		Smoky,
		Dust,
		Snow,
		Hail,
		Sleet,
		FreezingDrizzle,
		FreezingRain,
		MixedRainAndHail,
		MixedRainAndSnow,
		MixedRainAndSleet,
		MixedSnowAndSleet,
		Drizzle,
		ScatteredShowers,
		Showers,
		Thunderstorms,
		TropicalStorm,
		Hurricane,
		Tornado,
	}

	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKWorkoutSwimmingLocationType : long {
		Unknown = 0,
		Pool,
		OpenWater,
	}

	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKSwimmingStrokeStyle : long {
		Unknown = 0,
		Mixed,
		Freestyle,
		Backstroke,
		Breaststroke,
		Butterfly,
		[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
		Kickboard = 6,
	}

	/// <summary>Enumerates the reasons for why insulin was provided.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKInsulinDeliveryReason : long {
		Basal = 1,
		Bolus,
#if !NET
		[Obsolete ("Use 'Basal' instead.")]
		Asal = Basal,
		[Obsolete ("Use 'Bolus' instead.")]
		Olus = Bolus,
#endif
	}

	/// <summary>Enumerates values that tell whether a blood glucose level was taken before or after a meal.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKBloodGlucoseMealTime : long {
		/// <summary>Indicates a measurement taken before a meal.</summary>
		Preprandial = 1,
		/// <summary>Indicates a measurement taken after a meal.</summary>
		Postprandial,
#if !NET
		[Obsolete ("Use 'Preprandial' instead.")]
		Reprandial = Preprandial,
		[Obsolete ("Use 'Postprandial' instead.")]
		Ostprandial = Postprandial,
#endif
	}

	/// <summary>Enumerates the testing process used for establishing VO2 Max.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKVO2MaxTestType : long {
		MaxExercise = 1,
		PredictionSubMaxExercise,
		PredictionNonExercise,
	}

	/// <summary>Enumerates Fast Healthcare Interoperability Resources (FHIR) types.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	public enum HKFhirResourceType {
		[Field ("HKFHIRResourceTypeAllergyIntolerance")]
		AllergyIntolerance,
		[Field ("HKFHIRResourceTypeCondition")]
		Condition,
		[Field ("HKFHIRResourceTypeImmunization")]
		Immunization,
		[Field ("HKFHIRResourceTypeMedicationDispense")]
		MedicationDispense,
		[Field ("HKFHIRResourceTypeMedicationOrder")]
		MedicationOrder,
		[Field ("HKFHIRResourceTypeMedicationStatement")]
		MedicationStatement,
		[Field ("HKFHIRResourceTypeObservation")]
		Observation,
		[Field ("HKFHIRResourceTypeProcedure")]
		Procedure,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("HKFHIRResourceTypeMedicationRequest")]
		MedicationRequest,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("HKFHIRResourceTypeCoverage")]
		Coverage,
		[iOS (16, 4), MacCatalyst (16, 4), Mac (13, 3)]
		[Field ("HKFHIRResourceTypeDiagnosticReport")]
		DiagnosticReport,
		[iOS (16, 4), MacCatalyst (16, 4), Mac (13, 3)]
		[Field ("HKFHIRResourceTypeDocumentReference")]
		DocumentReference,
	}

	/// <summary>Enumerates clinical record type identifiers.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	public enum HKClinicalTypeIdentifier {

		/// <summary>Indicates an allergy record.</summary>
		[Field ("HKClinicalTypeIdentifierAllergyRecord")]
		AllergyRecord,
		/// <summary>Indicates a record for a medical condition.</summary>
		[Field ("HKClinicalTypeIdentifierConditionRecord")]
		ConditionRecord,
		/// <summary>Indicates an immunization record.</summary>
		[Field ("HKClinicalTypeIdentifierImmunizationRecord")]
		ImmunizationRecord,
		/// <summary>Indicates a lab result record.</summary>
		[Field ("HKClinicalTypeIdentifierLabResultRecord")]
		LabResultRecord,
		/// <summary>Indicates a record of medication</summary>
		[Field ("HKClinicalTypeIdentifierMedicationRecord")]
		MedicationRecord,
		/// <summary>Indicates a record of a procedure.</summary>
		[Field ("HKClinicalTypeIdentifierProcedureRecord")]
		ProcedureRecord,
		/// <summary>Indicates a record of a vital sign.</summary>
		[Field ("HKClinicalTypeIdentifierVitalSignRecord")]
		VitalSignRecord,
		[iOS (14, 0)]
		[MacCatalyst (14, 0)]
		[Field ("HKClinicalTypeIdentifierCoverageRecord")]
		CoverageRecord,
		[iOS (16, 4), Mac (13, 3)]
		[MacCatalyst (16, 4)]
		[Field ("HKClinicalTypeIdentifierClinicalNoteRecord")]
		ClinicalNoteRecord,
	}

	/// <summary>Enumerates values that tell when an app should request user permission for access.</summary>
	[Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKAuthorizationRequestStatus : long {
		/// <summary>Indicates that it is unknown whether the app should request user permission.</summary>
		Unknown = 0,
		/// <summary>Indicates that the app should request user permission.</summary>
		ShouldRequest,
		/// <summary>Indicates that the app does not need to request user permission.</summary>
		Unnecessary,
	}

	[iOS (13, 6), Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueAppetiteChanges : long {
		Unspecified = 0,
		NoChange,
		Decreased,
		Increased,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum HKAppleEcgAlgorithmVersion : long {
		Version1 = 1,
		Version2 = 2,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum HKCategoryValueEnvironmentalAudioExposureEvent : long {
		MomentaryLimit = 1,
	}

	[iOS (13, 6), Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValuePresence : long {
		Present = 0,
		NotPresent,
	}

	[iOS (13, 6), Mac (13, 0)]
	[MacCatalyst (13, 1)]
	[Native]
	public enum HKCategoryValueSeverity : long {
		Unspecified = 0,
		NotPresent,
		Mild,
		Moderate,
		Severe,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum HKDevicePlacementSide : long {
		Unknown = 0,
		Left,
		Right,
		Central,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum HKElectrocardiogramClassification : long {
		NotSet = 0,
		SinusRhythm,
		AtrialFibrillation,
		InconclusiveLowHeartRate,
		InconclusiveHighHeartRate,
		InconclusivePoorReading,
		InconclusiveOther,
		Unrecognized = 100,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum HKElectrocardiogramLead : long {
		AppleWatchSimilarToLeadI = 1,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	[Native]
	public enum HKElectrocardiogramSymptomsStatus : long {
		NotSet = 0,
		None = 1,
		Present = 2,
	}

	[iOS (14, 0), Mac (13, 0)]
	[MacCatalyst (14, 0)]
	public enum HKFhirRelease {
		[Field ("HKFHIRReleaseDSTU2")]
		Dstu2,
		[Field ("HKFHIRReleaseR4")]
		R4,
		[Field ("HKFHIRReleaseUnknown")]
		Unknown,
	}

	[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
	[Native]
	public enum HKHeartRateRecoveryTestType : long {
		MaxExercise = 1,
		PredictionSubMaxExercise,
		PredictionNonExercise,
	}

	[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
	[Native]
	public enum HKPrismBase : long {
		None = 0,
		Up,
		Down,
		In,
		Out,
	}

	[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
	[Native]
	public enum HKUserMotionContext : long {
		NotSet = 0,
		Stationary,
		Active,
	}

	[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
	[Native]
	public enum HKVisionEye : long {
		Left = 1,
		Right,
	}

	[MacCatalyst (16, 0), Mac (13, 0), iOS (16, 0), NoTV]
	[Native]
	public enum HKVisionPrescriptionType : ulong {
		Glasses = 1,
		Contacts,
	}

	[MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
	[Native]
	public enum HKCyclingFunctionalThresholdPowerTestType : long {
		MaxExercise60Minute = 1,
		MaxExercise20Minute,
		RampTest,
		PredictionExercise,
	}

	[MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
	[Native]
	public enum HKPhysicalEffortEstimationType : long {
		ActivityLookup = 1,
		DeviceSensed,
	}

	[MacCatalyst (17, 0), Mac (14, 0), iOS (17, 0)]
	[Native]
	public enum HKWaterSalinity : long {
		FreshWater = 1,
		SaltWater,
	}

	[iOS (17, 0)]
	[Native]
	public enum HKWorkoutSessionType : long {
		Primary = 0,
		Mirrored,
	}
}
