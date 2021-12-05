﻿using Blazored.LocalStorage;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UI.Services.Exceptions;
using UI.Services.Interfaces;
using UI.Services.Models;

namespace UI.Components.AddClass
{
    public partial class AddClassComponent : ComponentBase
    {
        protected string value = String.Empty;
        protected string _errorMessage = String.Empty;
        protected string[] _errors;
        private List<ClassModel> deserializedValue = new List<ClassModel>();

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public ILocalStorageService LocalStorageService { get; set; }

        [Inject]
        public IClassHttpService ClassHttpService { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            await LocalStorageService.RemoveItemAsync("MyClasses");
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("initializeAddClass");
            }
        }

        protected async Task HandleJson()
        {
            value = await LocalStorageService.GetItemAsync<string>("MyClasses");
            try
            {
                deserializedValue = JsonConvert.DeserializeObject<List<ClassModel>>(value);
            }
            catch (Exception ex)
            {
                ToastService.ShowError("Nastąpił problem z serializacją danych");
            }
        }



        protected async Task HandleAddClass()
        {
            await HandleJson();
            await ComponentRequestHandler.HandleRequest<List<ClassModel>>
                (ClassHttpService.CreateClass, deserializedValue, _errorMessage, _errors, ToastService);
            //if (_errorMessage == String.Empty) { ToastService.ShowSuccess("Pomyślnie zapisano dane"); }
        }
    }
}
