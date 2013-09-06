﻿using Coevery.Core.ClientRoute;
using Orchard.Environment.Extensions.Models;

namespace Coevery.OptionSet.Services {
    public class ClientRouteProvider : ClientRouteProviderBase {
        public override void Discover(ClientRouteTableBuilder builder) {
            builder.Describe("FieldEdit")
                .Configure(descriptor => {
                    descriptor.Abstract = true;
                });
            builder.Describe("FieldEdit.Items")
                .View(view => {
                    view.TemplateUrl = "'SystemAdmin/OptionSet/List'";
                    view.Controller = "OptionItemsCtrl";
                    view.Dependencies = ToClientUrl(new[] { "controllers/optionitemeditcontroller", "services/optionitemdataservice" });
                });
        }
    }
}