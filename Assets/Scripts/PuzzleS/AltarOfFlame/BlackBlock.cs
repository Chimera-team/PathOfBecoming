using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBlock : Block
{
    [SerializeField] public Block[] cancellingBlocks;
    [SerializeField] Sprite HalfSprite;
    [SerializeField] SpriteRenderer blackShitSpriteRen;
    BlockPuzzle bp;
    bool isStealingMana = false;
    
    private void Awake()
    {
        isBlackBlock = true;
        bp = FindObjectOfType<BlockPuzzle>();
        
    }
    public override int FillBlock(int currentMana)
    {
        //sound
        SoundRecorder.Play_Effect(bp.blackKill);
        blackShitSpriteRen.enabled = false;
       bp.RemoveBlocksThatCanBeSucked(cancellingBlocks);
        return base.FillBlock(currentMana);
    }
    public override void PartFillBlock(int strength)
    {
        //sound
        SoundRecorder.Play_Effect(bp.damageBlack);
        blackShitSpriteRen.sprite = HalfSprite;
        base.PartFillBlock(strength);
    }
    // Update is called once per frame
    void Update()
    {
        if(!isFilled && !isStealingMana)
        {
            foreach (Block block in cancellingBlocks)
            {
                if(block.isFilled)
                {
                    isStealingMana = true;
                    StartCoroutine( EatMana(block));
                    break;
                }
            }
        }
    }
    IEnumerator EatMana(Block block)
    {
        //sound
        SoundRecorder.Play_Effect(bp.suckMana);

        bp.AllowOrNoClicks(false);
        //block.PaintBlock(Color.yellow);
        //Debug.Log("tut");
        yield return new WaitForSeconds(.5f);
        block.PaintBlock(Color.red);
        yield return new WaitForSeconds(.5f);
        block.PaintBlock(Color.white);
        block.EmptyBlock();
        bp.AllowOrNoClicks(true);
        isStealingMana = false;
    }
}
