﻿using DinkToPdf.Contracts;
using DinkToPdf;
using Google.Protobuf.WellKnownTypes;
using Google.Protobuf;
using Grpc.Core;
using PdfService.Configurations;

namespace PdfService.Service
{
    public class PdfGenerationService : PdfGenerator.PdfGeneratorBase
    {
        private readonly IConverter _converter;
        private readonly HtmlToPdfConfiguration _configuration;

        public PdfGenerationService(IConverter converter, HtmlToPdfConfiguration configuration)
        {
            _converter = converter;
            _configuration = configuration;
        }

        public override async Task<PdfDocumentResponse> Generate(HtmlDocumentRequest request, ServerCallContext context)
        {
            _configuration.GlobalSettings.DocumentTitle = request.Name;
            _configuration.GlobalSettings.Out = "";
            _configuration.ObjectSettings.HtmlContent = request.Content;
            var document = new HtmlToPdfDocument() { GlobalSettings = _configuration.GlobalSettings, Objects = { _configuration.ObjectSettings } };
            return new PdfDocumentResponse()
            {
                Content = ByteString.CopyFrom(_converter.Convert(document)),
                Created = Timestamp.FromDateTime(DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc))
            };
        }
    }
}
