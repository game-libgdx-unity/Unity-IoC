﻿using System;
using System.Collections;
using System.Collections.Generic;
using SceneTest;
using UnityEngine;
using UnityEngine.UI;
using UnityIoC;
using Object = UnityEngine.Object;

public class PlayerUI : MonoBehaviour, IObserver<PlayerData>
{
    [SerializeField, Inject] Text txtName;
    [SerializeField, Inject] Button btnDelete;

    public void OnCompleted()
    {
        Destroy(gameObject);
    }

    public void OnError(Exception error)
    {
        //Msg.Error(exception);
    }

    public void OnNext(PlayerData playerData)
    {
        txtName.text = playerData.name;
        btnDelete.onClick.AddListener(() => { Context.Delete(playerData); });
    }
}