using AutoMapper;
using Domain.AADE;
using Domain.DTO;
using Domain.Model;


namespace Business.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<MyDataInvoice, MyDataInvoiceDTO>().ReverseMap();
            CreateMap<MyDataCancelInvoice, MyDataCancelInvoiceDTO>().ReverseMap();

            CreateMap<MyDataCancelationResponse, MyDataCancelationResponseDTO>().ReverseMap();
                //.ForMember(d => d.MyDataInvoice, opt => opt.Ignore());

            CreateMap<MyDataResponse, MyDataResponseDTO>().ReverseMap()
                .ForMember(d => d.MyDataInvoice, opt => opt.Ignore());

            CreateMap<MyDataCancelationError, MyDataCancelationErrorDTO>().ReverseMap()
                .ForMember(d => d.MyDataCancelationResponse, opt => opt.Ignore());

            CreateMap<MyDataError, MyDataErrorDTO>().ReverseMap()
                .ForMember(d => d.MyDataResponse, opt => opt.Ignore());






            CreateMap<MyDataEntity, MyDataEntityDTO>().ReverseMap();
            
            
            
            CreateMap<MyDataInvoiceType, MyDataInvoiceTypeDTO>().ReverseMap();
            
            
            CreateMap<MyDataIncome,MyDataIncomeDTO>().ReverseMap();
            CreateMap<MyDataIncomeResponse, MyDataIncomeResponseDTO>().ReverseMap();
            CreateMap<MyDataIncomeError, MyDataIncomeErrorDTO>().ReverseMap();
            CreateMap<TaxInvoice, TaxInvoiceDTO>().ReverseMap();



            CreateMap<MyDataErrorDTO, MyGenericErrorsDTO>().ReverseMap();
            CreateMap<MyDataIncomeErrorDTO, MyGenericErrorsDTO>().ReverseMap();
            CreateMap<MyDataCancelationErrorDTO, MyGenericErrorsDTO>().ReverseMap();

            CreateMap<MyDataTransmittedDocInvoiceDTO, MyDataTransmittedDocInvoice>().ReverseMap();
            CreateMap<MyDataPartyTypeDTO, MyDataPartyType>().ReverseMap();
            CreateMap<MyDataAddressTypeDTO, MyDataAddressType>().ReverseMap();
            CreateMap<MyDataInvoiceHeaderTypeDTO, MyDataInvoiceHeaderType>().ReverseMap();
            CreateMap<MyDataPaymentMethodDetailDTO, MyDataPaymentMethodDetail>().ReverseMap();
            CreateMap<MyDataInvoiceRowTypeDTO, MyDataInvoiceRowType>().ReverseMap();            
            CreateMap<MyDataTaxesDTO, MyDataTaxes>().ReverseMap();
            CreateMap<MyDataInvoiceSummaryDTO, MyDataInvoiceSummary>().ReverseMap();
            CreateMap<MyDataIncomeClassificationDTO, MyDataIncomeClassification>().ReverseMap();
            CreateMap<MyDataExpenseTypeDTO, MyDataExpenseType>().ReverseMap();


            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<Branch, BranchDTO>().ReverseMap();
            CreateMap<Particle, ParticleDTO>().ReverseMap();
            CreateMap<Pmove, PmoveDTO>().ReverseMap();
            CreateMap<Ptyppar, PtypparDTO>().ReverseMap();
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<FPA, FPADTO>().ReverseMap();
            CreateMap<Client, ClientDTO>().ReverseMap();



            CreateMap<ResponseDocResponse, MyDataResponseDTO>();
            //CreateMap<ResponseDocResponse, MyDataCancelationResponseDTO>();
            CreateMap<ResponseDocResponseError, MyDataErrorDTO>();
            CreateMap<ResponseDocResponseError, MyDataCancelationErrorDTO>();
        }

       
    }
}

