using UnityEngine;
using System.Collections;

public class Levels : MonoBehaviour {

    int levelNumber = 1;
    static public Level currentLevel;

    public GameObject Splash;

    public void LoadNextLevel() {
        if (currentLevel != null) currentLevel.unLoadLevel();
        if ((levelNumber > 4 && levelNumber < 10) || levelNumber > 11) Splash.SetActive(true);
        Load(levelNumber++);
    }

    public void LoadTutorials()
    {
        levelNumber = 10;
        LoadNextLevel();
    }

    public void LoadSandBox() {
        if (currentLevel != null) currentLevel.unLoadLevel();
        levelNumber = 0;
        currentLevel = new SandBox();
    }

    public void LoadChallenges() {
        levelNumber = 1;
        LoadNextLevel();

    }

    void Load(int levelNumber) {
        if (levelNumber == 1) currentLevel = new Level1();
        if (levelNumber == 2) currentLevel = new Level2();
        if (levelNumber == 3) currentLevel = new Level3();
        if (levelNumber == 4) currentLevel = new Level4();

        if (levelNumber == 10) currentLevel = new Tutorial1();
        if (levelNumber == 11) currentLevel = new Tutorial2();
        
     //   if (levelNumber == 5) currentLevel = new Level5();
    }

    public void restart() {
        if (levelNumber == 0) LoadSandBox();
        levelNumber--;
        LoadNextLevel();
    }

    public void showInfo()
    {
        currentLevel.showInfo();
    }

}
