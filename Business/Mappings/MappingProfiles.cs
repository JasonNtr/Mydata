using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Database.RequestDocModels;

namespace Business.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MyDataEntity, MyDataEntityDTO>().ReverseMap();
            

            CreateMap<MyDataError, MyDataErrorDTO>().ReverseMap()
                .ForMember(d => d.MyDataResponse, opt => opt.Ignore());
            CreateMap<MyDataCancelationError, MyDataCancelationErrorDTO>().ReverseMap()
                .ForMember(d => d.MyDataCancelationResponse, opt => opt.Ignore());
            
            CreateMap<MyDataInvoice, MyDataInvoiceDTO>().ReverseMap();
            CreateMap<MyDataCancelInvoice, MyDataCancelInvoiceDTO>().ReverseMap();
            //.ForMember(d => d.MyDataResponses, opt => opt.Ignore());
            //.AfterMap(AddOrUpdateResponses);
            CreateMap<MyDataInvoiceType, MyDataInvoiceTypeDTO>().ReverseMap();
            CreateMap<MyDataResponse, MyDataResponseDTO>().ReverseMap()
                .ForMember(d => d.MyDataInvoice, opt => opt.Ignore());
            CreateMap<MyDataCancelationResponse, MyDataCancelationResponseDTO>().ReverseMap()
                .ForMember(d => d.MyDataInvoice, opt => opt.Ignore());
            //.ForMember(d => d.Errors, opt => opt.Ignore()); ;


            CreateMap<MyDataIncome,MyDataIncomeDTO>().ReverseMap();
            CreateMap<MyDataIncomeResponse, MyDataIncomeResponseDTO>().ReverseMap();
            CreateMap<MyDataIncomeError, MyDataIncomeErrorDTO>().ReverseMap();



            CreateMap<MyDataErrorDTO, MyGenericErrorsDTO>().ReverseMap();
            CreateMap<MyDataIncomeErrorDTO, MyGenericErrorsDTO>().ReverseMap();
            CreateMap<MyDataCancelationErrorDTO, MyGenericErrorsDTO>().ReverseMap();

            CreateMap<MyDataTransmittedDocInvoiceDTO, MyDataTransmittedDocInvoice>().ReverseMap();
            CreateMap<MyDataPartyTypeDTO, MyDataPartyType>().ReverseMap();
            CreateMap<MyDataInvoiceRowTypeDTO, MyDataInvoiceRowType>().ReverseMap();
            CreateMap<MyDataAddressTypeDTO, MyDataAddressType>().ReverseMap();
            CreateMap<MyDataInvoiceHeaderTypeDTO, MyDataInvoiceHeaderType>().ReverseMap();
            CreateMap<MyDataPaymentMethodDetailDTO, MyDataPaymentMethodDetail>().ReverseMap();
            CreateMap<MyDataTaxesDTO, MyDataTaxes>().ReverseMap();
            CreateMap<MyDataInvoiceSummaryDTO, MyDataInvoiceSummary>().ReverseMap();
            CreateMap<MyDataIncomeClassificationDTO, MyDataIncomeClassification>().ReverseMap();
            
        }

       
    }
}

