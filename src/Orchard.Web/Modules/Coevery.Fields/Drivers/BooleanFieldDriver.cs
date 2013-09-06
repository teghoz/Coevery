﻿using System;
using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.Handlers;
using Coevery.Fields.Fields;
using Coevery.Fields.Settings;
using Orchard.Localization;

namespace Coevery.Fields.Drivers {
    public class BooleanFieldDriver : ContentFieldDriver<BooleanField> {
        public IOrchardServices Services { get; set; }
        private const string TemplateName = "Fields/Boolean.Edit";

        public BooleanFieldDriver(IOrchardServices services) {
            Services = services;
            T = NullLocalizer.Instance;
            DisplayName = "Boolean";
            Description = "Allows users to enter any combination of letters and numbers.";
        }

        public Localizer T { get; set; }

        private static string GetPrefix(ContentField field, ContentPart part) {
            return part.PartDefinition.Name + "." + field.Name;
        }

        private static string GetDifferentiator(BooleanField field, ContentPart part) {
            return field.Name;
        }

        protected override DriverResult Display(ContentPart part, BooleanField field, string displayType, dynamic shapeHelper) {
            return ContentShape("Fields_Boolean", GetDifferentiator(field, part), () => {
                var settings = field.PartFieldDefinition.Settings.GetModel<BooleanFieldSettings>();
                return shapeHelper.Fields_Boolean().Settings(settings);
            });
        }

        protected override DriverResult Editor(ContentPart part, BooleanField field, dynamic shapeHelper) {
            // if the content item is new, assign the default value

            if (!field.Value.HasValue) {
                var settings = field.PartFieldDefinition.Settings.GetModel<BooleanFieldSettings>();
                field.Value = settings.DefaultValue;
            }

            return ContentShape("Fields_Boolean_Edit", GetDifferentiator(field, part),
                                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: field, Prefix: GetPrefix(field, part)));
        }

        protected override DriverResult Editor(ContentPart part, BooleanField field, IUpdateModel updater, dynamic shapeHelper) {
            if (updater.TryUpdateModel(field, GetPrefix(field, part), null, null)) {
                var settings = field.PartFieldDefinition.Settings.GetModel<BooleanFieldSettings>();
                if (settings.Required && !field.Value.HasValue) {
                    updater.AddModelError(GetPrefix(field, part), T("The field {0} is mandatory.", T(field.DisplayName)));
                }
            }
            return Editor(part, field, shapeHelper);
        }

        protected override void Importing(ContentPart part, BooleanField field, ImportContentContext context) {
            context.ImportAttribute(field.FieldDefinition.Name + "." + field.Name, "Value", v => field.Value = bool.Parse(v));
        }

        protected override void Exporting(ContentPart part, BooleanField field, ExportContentContext context) {
            context.Element(field.FieldDefinition.Name + "." + field.Name).SetAttributeValue("Value", field.Value);
        }

        protected override void Describe(DescribeMembersContext context) {
            context
                .Member(null, typeof(bool), T("Value"), T("The boolean value of the field."));
        }
    }
}