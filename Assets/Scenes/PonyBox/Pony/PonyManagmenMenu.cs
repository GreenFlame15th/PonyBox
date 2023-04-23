using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PonyManagmenMenu : MonoBehaviour
{
    #region selecion
    public bool muiltSelect;

    public void SetMuiltSelect(bool muiltSelect)
    {
        this.muiltSelect = muiltSelect;
        if (!muiltSelect)
        {
            List<UnifiedPonyObject> currentlySelected = PonyBoxManager.instance.ponies.FindAll(upo => upo.ponyGridElement.selected);
            if (currentlySelected.Count > 1)
            {
                for (int i = 1; i < currentlySelected.Count; i++)
                {
                    currentlySelected[i].ponyGridElement.Toggle(false);
                }
            }
        }

    }

    public void Toggle(PonyGridElement toSelect, bool on)
    {
        if (on && !muiltSelect)
        {
            UnifiedPonyObject upo = PonyBoxManager.instance.ponies.Find(upo => upo.ponyGridElement.selected);
            if (upo != null)
            {
                upo.ponyGridElement.Toggle(false);
            }
        }
    }

    public MiltySelectElement muiltSelectOnToggle;
    public void ToggleAll(bool on)
    {
        foreach (UnifiedPonyObject upo in PonyBoxManager.instance.ponies)
        {
            upo.ponyGridElement.Toggle(on);
        }

        if (on && !muiltSelect)
        {
            muiltSelectOnToggle.Click();
        }
    }

    public void ToggleRandome()
    {
        List<UnifiedPonyObject> currentlyNoySelected = PonyBoxManager.instance.ponies.FindAll(upo => !upo.ponyGridElement.selected);
        if (currentlyNoySelected.Count > 0)
        {
            PonyGridElement target = currentlyNoySelected[Random.Range(0, currentlyNoySelected.Count)].ponyGridElement;
            Toggle(target, true);
            target.Toggle(true);
        }

    }
    #endregion

    public List<UnifiedPonyObject> CurrenctlySelected() { return PonyBoxManager.instance.ponies.FindAll(upo => upo.ponyGridElement.selected); }

    public void spawnSelected(int count)
    {
        if (count > 0)
        {
            List<UnifiedPonyObject> currenctlySelected = CurrenctlySelected();

            if (currenctlySelected.Count > 0)
            {
                //special behavour for first enqueue
                if (PonyBoxManager.instance.spriteMaker.ponyQueue.Any())
                {
                    currenctlySelected[0].addInstance();
                }
                else
                {
                    currenctlySelected[0].enqueueInstance();
                }

                for (int i = 1; i < currenctlySelected.Count; i++)
                {
                    currenctlySelected[i].enqueueInstance();
                }

                //enqueue all
                for (int i = 1; i < count; i++)
                {
                    for (int j = 0; j < currenctlySelected.Count; j++)
                    {
                        currenctlySelected[j].enqueueInstance();
                    }
                }
            }

        }
    }

    public int destroyWorning;
    public void removeNFromSelected(int n)
    {
        List<UnifiedPonyObject> currenctlySelected = CurrenctlySelected();
        if (currenctlySelected.Count > 0)
        {
            int toDestroy = 0;
            foreach (UnifiedPonyObject upo in currenctlySelected)
            {
                toDestroy += upo.canDestroy(n);
            }

            if (toDestroy >= destroyWorning)
            {
                PonyBoxManager.instance.areYouSurePopUp.Invoke(
                    "You are about to destroy " + toDestroy + " ponies, are you sure?", () =>
                    {
                        foreach (UnifiedPonyObject upo in currenctlySelected)
                        {
                            upo.destryoNInstances(n);
                        }
                    }
                    );
            }
            else
            {
                foreach (UnifiedPonyObject upo in currenctlySelected)
                {
                    upo.destryoNInstances(n);
                }
            }
        }
    }
    public void removeAllFromSelected()
    {
        List<UnifiedPonyObject> currenctlySelected = CurrenctlySelected();
        if (currenctlySelected.Count > 0)
        {
            int toDestroy = 0;
            foreach (UnifiedPonyObject upo in currenctlySelected)
            {
                toDestroy += upo.instances.Count;
            }

            if (toDestroy >= destroyWorning)
            {
                PonyBoxManager.instance.areYouSurePopUp.Invoke(
                    "You are about to destroy " + toDestroy + " ponies, are you sure?", () =>
                    {
                        foreach (UnifiedPonyObject upo in currenctlySelected)
                        {
                            upo.destryoAllInstances();
                        }
                    }
                    );
            }
            else
            {
                foreach (UnifiedPonyObject upo in currenctlySelected)
                {
                    upo.destryoAllInstances();
                }
            }
        }
    }
    public void DelateAllSelected()
    {
        List<UnifiedPonyObject> currenctlySelected = CurrenctlySelected();
        if (currenctlySelected.Count > 0)
        {
         PonyBoxManager.instance.areYouSurePopUp.Invoke(
                "You are about to delete " + currenctlySelected.Count + " ponies, this will remove all instances and can only be undone by importing them again. Are you sure?", () =>
                {
                    foreach (UnifiedPonyObject upo in currenctlySelected)
                    {
                        upo.beGone();
                    }
                }
                );
        }
    }
    public Text delayText;
    public Slider delaySlider;
    public void UpdateSpawnDelay(float delay)
    {
        delay *= 0.5f;
        PonyBoxManager.instance.spriteMaker.spawnDelay = delay;
        delayText.text = "Span delay: " + delay + "s";
    }

    public void UpdateSpawnDelay()
    {
        UpdateSpawnDelay(delaySlider.value);
    }
}
