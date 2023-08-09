using MelonLoader;
using UnityEngine;
using ABI.CCK.Components;
using System.Linq;
using System.Collections.Generic;

using ActionMenu;
using ABI_RC.Core.Savior;
using ABI_RC.Systems.GameEventSystem;

[assembly:MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly:MelonInfo(typeof(PetAI.PetAIMod), "PetAI", "1.0.0", "daky", "https://github.com/dakyneko/DakyModsCVR")]
[assembly:MelonAdditionalDependencies("ActionMenu")]

namespace PetAI
{
    public class PetAIMod : MelonMod
    {
        private static MelonLogger.Instance logger;
        private static PetAIMod instance;
        private Menu menu;

        public override void OnInitializeMelon()
        {
            logger = LoggerInstance;
            instance = this;
			
            menu = new Menu();

            CVRGameEventSystem.Spawnable.OnInstantiate.AddListener((ownerId, spawnable) =>
            {
                if (ownerId != MetaPort.Instance.ownerId) return; // only own props
                var t = spawnable.transform.Find("PetAI_Agent"); // sentinel object
                if (t == null) return;

                var pet = t.gameObject.AddComponent<PuPet>();
                pet.Init(this, logger);
            });
        }
		
        public class Menu : ActionMenu.ActionMenuMod.Lib
        {
            protected override string modName => "PetAI";
            protected override string? modIcon => "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAMAAABg3Am1AAAAxHpUWHRSYXcgcHJvZmlsZSB0eXBlIGV4aWYAAHjabVBRDsMgCP33FDsCAiocx65tshvs+EOk3drsJTyRR55I2t6vPT0GMHPi0qRqrWBgZcVuicBEd87Azg4Kye6XejoFtBJ9O6VG/1HPp8E8umXlx0ieISxXQTn85WYUD9GYCC1Zw0jDiHAKOQz6/BZUlfb7hWWDK2RGGqQVdjcL9/udm21vLfYOIW6UCYyJeA5AIyhRt0ScizVmzzOxV1pMYgv5t6cD6QNJWFnygwkZywAAAYRpQ0NQSUNDIHByb2ZpbGUAAHicfZE9SMNAHMVfU0uLtCjYQcQhQ3Wyi4o4lioWwUJpK7TqYHLpFzQxJCkujoJrwcGPxaqDi7OuDq6CIPgB4uripOgiJf4vKbSI8eC4H+/uPe7eAUKrzlSzLwGommVkU0mxUFwRg68IIYBBRBCUmKmncwt5eI6ve/j4ehfnWd7n/hwRpWQywCcSJ5huWMTrxDObls55nzjKqpJCfE48YdAFiR+5Lrv8xrnisMAzo0Y+O0ccJRYrPSz3MKsaKvE0cUxRNcoXCi4rnLc4q/UG69yTvzBc0pZzXKc5ihQWkUYGImQ0UEMdFuK0aqSYyNJ+0sM/4vgz5JLJVQMjxzw2oEJy/OB/8Ltbszw16SaFk0DgxbY/xoDgLtBu2vb3sW23TwD/M3Cldf0bLWD2k/RmV4sdAQPbwMV1V5P3gMsdYPhJlwzJkfw0hXIZeD+jbyoCQ7dA/6rbW2cfpw9AnrpaugEODoHxCmWvebw71Nvbv2c6/f0ANdpyjjqkmwQAAA12aVRYdFhNTDpjb20uYWRvYmUueG1wAAAAAAA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/Pgo8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJYTVAgQ29yZSA0LjQuMC1FeGl2MiI+CiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiCiAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICB4bWxuczpHSU1QPSJodHRwOi8vd3d3LmdpbXAub3JnL3htcC8iCiAgICB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iCiAgICB4bWxuczp0aWZmPSJodHRwOi8vbnMuYWRvYmUuY29tL3RpZmYvMS4wLyIKICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgeG1wTU06RG9jdW1lbnRJRD0iZ2ltcDpkb2NpZDpnaW1wOjQ4ZDFkOTBlLWVlNzktNGM1My04NTZkLTUyNmM0ZGI5ODhkMSIKICAgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpkNTcxYmExNi0xMTBhLTRiY2ItYjBmYS1jNzVmN2Q1MzkzYTciCiAgIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDphMGQ0NGEzNC01MWU2LTQwZTYtYmRjOS1kYjQzZGM4NDc0MDEiCiAgIEdJTVA6QVBJPSIyLjAiCiAgIEdJTVA6UGxhdGZvcm09IldpbmRvd3MiCiAgIEdJTVA6VGltZVN0YW1wPSIxNjkxMjUyMDQ4NzE3MDQ3IgogICBHSU1QOlZlcnNpb249IjIuMTAuMzQiCiAgIGRjOkZvcm1hdD0iaW1hZ2UvcG5nIgogICB0aWZmOk9yaWVudGF0aW9uPSIxIgogICB4bXA6Q3JlYXRvclRvb2w9IkdJTVAgMi4xMCIKICAgeG1wOk1ldGFkYXRhRGF0ZT0iMjAyMzowODowNVQxODoxNDowNyswMjowMCIKICAgeG1wOk1vZGlmeURhdGU9IjIwMjM6MDg6MDVUMTg6MTQ6MDcrMDI6MDAiPgogICA8eG1wTU06SGlzdG9yeT4KICAgIDxyZGY6U2VxPgogICAgIDxyZGY6bGkKICAgICAgc3RFdnQ6YWN0aW9uPSJzYXZlZCIKICAgICAgc3RFdnQ6Y2hhbmdlZD0iLyIKICAgICAgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDplMjU5YTQ2MC01NTFkLTRhNjAtODQyNy0xM2U3OGY3ZGI3NGMiCiAgICAgIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkdpbXAgMi4xMCAoV2luZG93cykiCiAgICAgIHN0RXZ0OndoZW49IjIwMjMtMDgtMDVUMTg6MTQ6MDgiLz4KICAgIDwvcmRmOlNlcT4KICAgPC94bXBNTTpIaXN0b3J5PgogIDwvcmRmOkRlc2NyaXB0aW9uPgogPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5kPSJ3Ij8+mZAnCwAAAwBQTFRFAAAA////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAZ3bsYwAAAAF0Uk5TAEDm2GYAAAABYktHRACIBR1IAAAACXBIWXMAABmNAAAZjQEn17ZGAAAAB3RJTUUH5wgFEA4ITEU8pQAAAMBJREFUSMfVldsSgCAIROH/f7qaKWUXEHypyQfzskdG2Erk5aZnW80jvZHQNNLfsqGfq7n+GsAjJ1QpUgngSIONFFA490+AxoDL0lOpApiyUX+sHFflkc1OpieSmYlsbQRKDH4++BzvVoqGQOXLVwDZA4TSBLc2Y8olVVOxFxaF2bNxwnw7M6VWCt2aj1Ogsy759+dHwPald4EstJae7AaI95a+3wZkbbAOUelZUevB99rR+79oF+nL3RvcROSrdgBe6QFl+28VEgAAAABJRU5ErkJggg==";

            private PuPet pet;
            protected override void OnGlobalMenuLoaded(Menus menus)
            {
                ModsMainMenu(menus).Add(new MenuItem()
                {
                    name = modName,
                    icon = modIcon,
                    action = BuildCallbackMenu(modName, modMenuItems), // dynamic menu
                });
            }

            protected override List<MenuItem> modMenuItems()
            {
                var xs = new List<MenuItem>()
                {
                    new MenuItem("Select pet", BuildCallbackMenu("select_pet", () => GameObject.FindObjectsOfType<PuPet>().Select(
                        (p, i) => new MenuItem($"{p.name} #{i}", BuildButtonItem(p.name + i, () => pet = p))).ToList())),
                };
                if (pet != null)
                {
                    xs.AddRange(new List<MenuItem>() {
                        new MenuItem("Follow me", BuildToggleItem("followme", pet.ToggleFollowMe), pet.following != null),
                        new MenuItem("Follow", BuildCallbackMenu("follow_player", () =>
                            MetaPort.Instance.PlayerManager.NetworkPlayers.Select(
                                p => new MenuItem(p.Username, BuildButtonItem(p.Username, () => pet.FollowPlayer(p)))).ToList())),
                        new MenuItem("NavMesh Follow", BuildToggleItem("navmeshfollow", pet.ToggleNavMeshFollow), pet.agent?.enabled == true),
                        new MenuItem("Animate", BuildCallbackMenu("animate", () =>
                            (new List<string> { "Idle", "Licky", "Scratch", "Hop", "Stand" }).Select(
                                (x, i) => new MenuItem(x, BuildButtonItem(x, () => pet.Animate(i)))).ToList())),
                        new MenuItem("Mouth", BuildCallbackMenu("mouth", () =>
                            (new List<string> { "tongue", "Idle", ":O", ":/", ":(", ":<", ":>" }).Select(
                                (x, i) => new MenuItem(x, BuildButtonItem(x, () => pet.SetMouth(i-1)))).ToList())),
                        new MenuItem("Eyes", BuildCallbackMenu("eyes", () =>
                            (new List<string> { "Idle", "Happy", "Calm" }).Select(
                                (x, i) => new MenuItem(x, BuildButtonItem(x, () => pet.SetEyes(i)))).ToList())),
                        new MenuItem("Sound", BuildCallbackMenu("sound", () =>
                            (new List<string> { "None", "Kevie", "Meow" }).Select(
                                (x, i) => new MenuItem(x, BuildButtonItem(x, () => pet.SetSound(i)))).ToList())),
                    });
                }
                return xs;
            }
        }

    }
}
