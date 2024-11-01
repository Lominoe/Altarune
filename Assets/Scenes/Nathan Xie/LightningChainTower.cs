using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightningChainTower : Summon
{
    [SerializeField] private LightningChainBall ball;
    [SerializeField] private float attackInterval;
    private bool init;
    private float attackTick = -0.2f;
    private Player target;
    private Queue<LightningChainBall> balls = new Queue<LightningChainBall>();
    protected override void Awake() {
        base.Awake();
    }
    public override void Init(Player player) {
        base.Init(player);
        target = player;
        init = true;
    }
    protected override void Update() {
        Fire();
        base.Update();
    }   
    private void Fire(){
        if (!init) return;
        attackTick += Time.deltaTime;
        if (attackTick >= attackInterval && balls.Count < 2) {
            attackTick = 0;
            LightningChainBall projectile = Instantiate(ball, transform.position + Vector3.up, Quaternion.identity);
            projectile.OnTimeOut += RemoveBall;
            balls.Enqueue(projectile);
            projectile.Launch(transform.position, target.transform.position);
        }
    }
    private void RemoveBall(LightningChainBall ball){
        balls.Dequeue();
        if (attackTick >= attackInterval) attackTick = 0f;
    }
    public void OnDestroy(){
        while(balls.Count > 0){
            Debug.Log(balls.Count);
            Debug.Log(balls.Peek());
            balls.Peek().End();
        }
        balls.Clear();
    }
}
