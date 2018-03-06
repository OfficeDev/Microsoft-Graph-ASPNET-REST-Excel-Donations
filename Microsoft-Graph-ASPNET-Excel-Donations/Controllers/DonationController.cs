﻿//Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
//See LICENSE in the project root for license information.

using System.Collections.Generic;
using System.Web.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft_Graph_ASPNET_Excel_Donations.TokenStorage;
using Microsoft_Graph_ASPNET_Excel_Donations.Helpers;
using System.Configuration;

namespace Microsoft_Graph_ASPNET_Excel_Donations.Controllers
{
    public class DonationController : Controller
    {

        //
        // GET: ToDoList
        public async Task<ActionResult> Index()
        {
            string userObjId = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            SessionTokenCache tokenCache = new SessionTokenCache(userObjId, HttpContext);

            string accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

            return View(await ExcelApiHelper.GetDonations(accessToken));
        }

        // Create donation
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoList/Create
        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection)
        {
            try
            {

                string userObjId = System.Security.Claims.ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                SessionTokenCache tokenCache = new SessionTokenCache(userObjId, HttpContext);

                string accessToken = await SampleAuthProvider.Instance.GetUserAccessTokenAsync();

                await ExcelApiHelper.CreateDonation(
                    accessToken,
                    collection["Date"],
                    collection["Amount"],
                    collection["Organization"]);
                return RedirectToAction("Index");
            }
            catch
            {
                //Handle error
            }

            return View();
        }

    }
}
