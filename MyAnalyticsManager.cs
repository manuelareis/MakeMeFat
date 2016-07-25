using UnityEngine;
using System.Collections;
using GameAnalyticsSDK;

public class MyAnalyticsManager : MonoBehaviour {

    public static void PublishScore(int scorenum, int coins){
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Score", scorenum);
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Coin", coins);
    }
    /// <summary>
    /// Publish the performance of game match when finished 
    /// </summary>
    public static void PublishMatchPerformance(float gameDuration,int foodsQt, int badThingsQt, int powersQt, int coinsQt,
        int lostCoinsQt , int lostPowerQt, int lostFoodsQt, int lostBadthingsQt, float averageLifeValue, float lifeUp100time, float lifebtw30n70time , float lifeLess30time){

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "GameTime", (int)gameDuration);// duration of game match
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Foods Eaten", foodsQt);// how many foods eaten
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Bad Things Eaten ", badThingsQt);// how many bad things eaten 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Power ups eaten", powersQt);// how many power ups eaten
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Coins eaten", coinsQt);// how many coins eaten
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Coins lost", lostCoinsQt);// how many coins lost
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Power ups lost ", lostPowerQt);// how many power ups lost 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Foods lost ", lostFoodsQt);// how many foods lost 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Badthings avoided", lostBadthingsQt);// how many badthings avoided
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Average of life  ", (int)averageLifeValue);// average of life  
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Life over 100 ", (int)lifeUp100time);// duration of life over 100 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Life between 30 and 70", (int)lifebtw30n70time);// duration of life between 30 and 70
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Life less than 30", (int)lifeLess30time);// duration of life less than 30
    }

    /// <summary>
    /// Use to publish the performance when the player has not finished the match
    /// </summary>
    public static void PublishMatchStoppedPerformance(float gameDuration, int foodsQt, int badThingsQt, int powersQt, int coinsQt,
        int lostCoinsQt, int lostPowerQt, int lostFoodsQt, int lostBadthingsQt, float averageLifeValue, float lifeUp100time, float lifebtw30n70time, float lifeLess30time){

        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "GameTime", (int)gameDuration);// duration of game match
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Foods Eaten", foodsQt);// how many foods eaten
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Bad Things Eaten ", badThingsQt);// how many bad things eaten 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Power ups eaten", powersQt);// how many power ups eaten
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Coins eaten", coinsQt);// how many coins eaten
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Coins lost", lostCoinsQt);// how many coins lost
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Power ups lost ", lostPowerQt);// how many power ups lost 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Foods lost ", lostFoodsQt);// how many foods lost 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Badthings avoided", lostBadthingsQt);// how many badthings avoided
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Average of life  ", (int)averageLifeValue);// average of life  
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Life over 100 ", (int)lifeUp100time);// duration of life over 100 
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Life between 30 and 70", (int)lifebtw30n70time);// duration of life between 30 and 70
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Life less than 30", (int)lifeLess30time);// duration of life less than 30
    }

    public static void RegisterCustomEvent(string eventName){
        GameAnalytics.NewDesignEvent(eventName);
    }

    public static void RegisterCustomEvent(string eventName, float value){
        GameAnalytics.NewDesignEvent(eventName, value);
    }

    void OnApplicationQuit()
    {
        RegisterCustomEvent("Application quit");
    }


}
