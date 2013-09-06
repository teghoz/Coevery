﻿using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Web.Mvc;
using Coevery.Entities.Services;
using Coevery.Relationship.Fields;
using Orchard.ContentManagement;

namespace Coevery.Relationship.Projections {

    public class ReferenceFieldValeProvider : ContentFieldValueProvider<ReferenceField> {
        private readonly IContentManager _contentManager;

        public ReferenceFieldValeProvider(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public override object GetValue(ContentItem contentItem, ContentField field) {
            var value = field.Storage.Get<int?>(null);

            if (value == null) {
                return null;
            }

            var referenceContentItem = _contentManager.Get(value.Value);
            var contentItemMetadata = _contentManager.GetItemMetadata(referenceContentItem);

            return contentItemMetadata.DisplayText;

            var pluralService = PluralizationService.CreateService(new CultureInfo("en-US"));
            string pluralContentTypeName = pluralService.Pluralize(referenceContentItem.ContentType);

            var linkTag = new TagBuilder("a");
            linkTag.Attributes.Add("href", "#/" + pluralContentTypeName + "/View/" + value);
            linkTag.InnerHtml = contentItemMetadata.DisplayText;
            return linkTag.ToString();
        }
    }
}