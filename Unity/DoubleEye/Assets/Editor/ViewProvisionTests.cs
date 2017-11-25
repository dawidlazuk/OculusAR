using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using Assets.Transmitter;
using Emgu.CV;
using ViewProvision;

public class ViewProvisionTests
{

    [Test]
    public void GetCurrentView_ShouldReturnNotNull()
    {
        //arrange
        var viewProvider = new ViewProvider();

        //act
        var result = viewProvider.GetCurrentView();

        //assert
        Assert.IsNotNull(result);
    }

}

