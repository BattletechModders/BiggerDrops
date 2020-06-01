using Newtonsoft.Json;
using System.Collections.Generic;
using BattleTech;
using System;

namespace BiggerDrops {
  public class Settings {

    public static readonly int MAX_ADDITINAL_MECH_SLOTS = 4;
    public int MAX_VEHICLE_SLOTS = 8;
    public static readonly int DEFAULT_MECH_SLOTS = 4;
    public static readonly string EMPLOYER_LANCE_GUID = "ecc8d4f2-74b4-465d-adf6-84445e5dfc230";
    public static readonly string ADDITIONAL_MECH_STAT = "BiggerDrops_AdditionalMechSlots";
    public static readonly string ADDITIONAL_PLAYER_MECH_STAT = "BiggerDrops_AdditionalPlayerMechSlots";
    public static readonly string MAX_TONNAGE_STAT = "BiggerDrops_MaxTonnage";
    public static readonly string LANCES_CONFIG_STAT_NAME = "BiggerDrops_LancesLayout";
    public static readonly string CU_VEHICLE_STAT_NAME = "BiggerDrops_CuVehicleCount";
    public static readonly int MIN_CU_MECHBAYS = 3;
    public int MAX_CU_DROP_SIZE = 12;
    public int MAX_CU_ADDITINAL_MECH_SLOTS = 8;

        public bool debugLog { get; set; }
    public bool debugLanceLoadout { get; set; }
    public int skirmishMax { get; set; }
    public string additionalLanceName { get; set; }
    public bool allowUpgrades { get; set; }
    public bool showAdditionalArgoUpgrades { get; set; }
    public string argoUpgradeName { get; set; }
    public string argoUpgradeCategory1Name { get; set; }
    public string argoUpgradeCategory2Name { get; set; }
    public string argoUpgradeCategory3Name { get; set; }
    public int CuInitialVehicles { get; set; }
    public bool respectFourDropLimit {get; set;}
    public bool limitFlashpointDrop {get; set;}
    public bool count4AsUnlimited {get; set;}


  public int additinalMechSlots {
      get {
        if (allowUpgrades && companyStats != null)
        {
            int maxSize = CustomUnitsAPI.Detected() ? MAX_CU_ADDITINAL_MECH_SLOTS : MAX_ADDITINAL_MECH_SLOTS;
            int val = companyStats.GetValue<int>(ADDITIONAL_MECH_STAT);
            FadditinalMechSlots = val > maxSize ? maxSize : val;
        }
        if (FadditinalMechSlots < 0) { FadditinalMechSlots = 0; }       
        return FadditinalMechSlots;
      }
      set {
        int maxSize = CustomUnitsAPI.Detected() ? MAX_CU_ADDITINAL_MECH_SLOTS : MAX_ADDITINAL_MECH_SLOTS;
        FadditinalMechSlots = value > maxSize ? maxSize : value;
        if (FadditinalPlayerMechSlots == -1) { FadditinalPlayerMechSlots = 0; } else
          if (FadditinalPlayerMechSlots > FadditinalMechSlots) { FadditinalPlayerMechSlots = FadditinalMechSlots; };
        FdefaultMechSlots = FadditinalMechSlots;
      }
    }
    public int additinalPlayerMechSlots {
      get {
        if (allowUpgrades && companyStats != null)
        {
            int maxSize = CustomUnitsAPI.Detected() ? MAX_CU_ADDITINAL_MECH_SLOTS : MAX_ADDITINAL_MECH_SLOTS;
            int val = companyStats.GetValue<int>(ADDITIONAL_PLAYER_MECH_STAT);
            FadditinalPlayerMechSlots = val > maxSize ? maxSize : val;
        }
        if (FadditinalPlayerMechSlots < 0) { FadditinalPlayerMechSlots = 0; }
        return FadditinalPlayerMechSlots;
      }
      set {
        int maxSize = CustomUnitsAPI.Detected() ? MAX_CU_ADDITINAL_MECH_SLOTS : MAX_ADDITINAL_MECH_SLOTS;
        FadditinalPlayerMechSlots = value > maxSize ? maxSize : value;
        if (FadditinalMechSlots == -1) { FadditinalMechSlots = FadditinalPlayerMechSlots; } else
          if (FadditinalMechSlots < FadditinalPlayerMechSlots) { FadditinalPlayerMechSlots = FadditinalMechSlots; };
        FdefaultPlayerMechSlots = FadditinalPlayerMechSlots;
      }
    }
    [JsonIgnore]
    public int vehicleCount
    {
        get
        {
            if (allowUpgrades && companyStats != null)
            {
                int val = companyStats.GetValue<int>(CU_VEHICLE_STAT_NAME);
                return val > MAX_VEHICLE_SLOTS ? MAX_VEHICLE_SLOTS : val;
            }
            return CuInitialVehicles;
        }
    }


    public int defaultMaxTonnage {
        get {
            if (allowUpgrades && companyStats != null)
            {
                FmaxTonnage = companyStats.GetValue<int>(MAX_TONNAGE_STAT);
            }
            return FmaxTonnage;
        }
        set {
            FmaxTonnage = value;
            FdefaultTonnage = value;
        }
    }

    [JsonIgnore]
    private int FadditinalMechSlots;
    [JsonIgnore]
    private int FadditinalPlayerMechSlots;
    [JsonIgnore]
    private int FmaxTonnage;
    [JsonIgnore]
    private int FdefaultMechSlots { get; set; }
    [JsonIgnore]
    private int FdefaultPlayerMechSlots;
    [JsonIgnore]
    private int FdefaultTonnage;
    [JsonIgnore]
    private StatCollection companyStats;
    public Settings() {
      debugLog = false;
      debugLanceLoadout = false;
      FadditinalMechSlots = -1;
      FadditinalPlayerMechSlots = -1;
      additionalLanceName = "AI LANCE";
      FmaxTonnage = 500;
      allowUpgrades = false;
      showAdditionalArgoUpgrades = false;
      argoUpgradeName = "Command & Control";
      argoUpgradeCategory1Name = "Drop Size";
      argoUpgradeCategory2Name = "Mech Control";
      argoUpgradeCategory3Name = "Drop Tonnage";
      CuInitialVehicles = 0;
      limitFlashpointDrop = true;
      respectFourDropLimit = false;
      count4AsUnlimited = true;
    }
    
    public void UpdateCULances() {
      if (CustomUnitsAPI.Detected()) {
        int lanceCount = 1;
        int mCount = BiggerDrops.
                    .additinalMechSlots;
        while (mCount > 0)
        {
            lanceCount++;
            mCount -= 4;
        }
        int vCount = BiggerDrops.settings.vehicleCount;
        while (vCount > 0)
        {
            lanceCount++;
            vCount -= 4;
        }
        int iBayCount = Math.Max(MIN_CU_MECHBAYS, companyStats.GetValue<int>("MechBayPods"));
        if (debugLanceLoadout) {
          CustomUnitsAPI.setLancesCount(3);
          CustomUnitsAPI.setLanceData(0, 6, 5, false);
          CustomUnitsAPI.setLanceData(1, 4, 4, false);
          CustomUnitsAPI.setLanceData(2, 4, 2, true);
          CustomUnitsAPI.setOverallDeployCount(6);
          CustomUnitsAPI.playerControl(-1, -1);
        } else {
          CustomUnitsAPI.setLancesCount(lanceCount);
          CustomUnitsAPI.setLanceData(0, DEFAULT_MECH_SLOTS, DEFAULT_MECH_SLOTS, false);
          int vStart = 1;
          mCount = BiggerDrops.settings.additinalMechSlots;
          while (mCount > 0)
          {
            CustomUnitsAPI.setLanceData(vStart, DEFAULT_MECH_SLOTS, mCount > DEFAULT_MECH_SLOTS ? DEFAULT_MECH_SLOTS : mCount, false);
            vStart++;
            mCount -= 4;          
           }
          vCount = BiggerDrops.settings.vehicleCount;
          while (vCount > 0)
          {
            CustomUnitsAPI.setLanceData(vStart, DEFAULT_MECH_SLOTS, vCount > DEFAULT_MECH_SLOTS ? DEFAULT_MECH_SLOTS : vCount, true);
            vStart++;
            vCount -= 4;
          }
          CustomUnitsAPI.setOverallDeployCount(System.Math.Min(DEFAULT_MECH_SLOTS + BiggerDrops.settings.additinalMechSlots + BiggerDrops.settings.vehicleCount, MAX_CU_DROP_SIZE));
          CustomUnitsAPI.playerControl(DEFAULT_MECH_SLOTS + BiggerDrops.settings.additinalPlayerMechSlots, BiggerDrops.settings.vehicleCount);
          CustomUnitsAPI.setMechBayCount(iBayCount);
        }
      }
    }

    public void setCompanyStats(StatCollection stats) {
            companyStats = stats;
            if (allowUpgrades) {

                if (!companyStats.ContainsStatistic(ADDITIONAL_MECH_STAT)) { companyStats.AddStatistic(ADDITIONAL_MECH_STAT, FdefaultMechSlots); };
                if (!companyStats.ContainsStatistic(ADDITIONAL_PLAYER_MECH_STAT)) { companyStats.AddStatistic(ADDITIONAL_PLAYER_MECH_STAT, FdefaultPlayerMechSlots); };
                if (!companyStats.ContainsStatistic(MAX_TONNAGE_STAT)) { companyStats.AddStatistic(MAX_TONNAGE_STAT, FdefaultTonnage); };
                if (!companyStats.ContainsStatistic(CU_VEHICLE_STAT_NAME)) { companyStats.AddStatistic(CU_VEHICLE_STAT_NAME, CuInitialVehicles); };
                UpdateCULances();
            }
     }
  }

  public class Fields {
    public static List<string> callsigns = new List<string>();
  }
}