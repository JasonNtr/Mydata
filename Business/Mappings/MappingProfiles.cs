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

            CreateMap<MyDataCancelationResponse, MyDataCancelationResponseDTO>().ReverseMap()
                .ForMember(d => d.MyDataInvoice, opt => opt.Ignore());

            CreateMap<MyDataResponse, MyDataResponseDTO>().ReverseMap()
                .ForMember(d => d.MyDataInvoice, opt => opt.Ignore());

            CreateMap<MyDataCancelationError, MyDataCancelationErrorDTO>().ReverseMap()
                .ForMember(d => d.MyDataCancelationResponse, opt => opt.Ignore());

            CreateMap<MyDataError, MyDataErrorDTO>().ReverseMap()
                .ForMember(d => d.MyDataResponse, opt => opt.Ignore());

            CreateMap<MeasurementUnit, MeasurementUnitDTO>().ReverseMap();
            CreateMap<MovePurpose, MovePurposeDTO>().ReverseMap();

            CreateMap<Company, CompanyDTO>().ReverseMap();
            CreateMap<MyDataEntity, MyDataEntityDTO>().ReverseMap();
            CreateMap<MyDataInvoiceType, MyDataInvoiceTypeDTO>().ReverseMap();
            CreateMap<MyDataIncome,MyDataIncomeDTO>().ReverseMap();
            CreateMap<MyDataIncomeResponse, MyDataIncomeResponseDTO>().ReverseMap();
            CreateMap<MyDataIncomeError, MyDataIncomeErrorDTO>().ReverseMap();
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
              CreateMap<Particle, ParticleDTO>().ReverseMap();
            CreateMap<PMove, PMoveDTO>().ReverseMap();
            CreateMap<InvoiceType, InvoiceTypeDTO>().ReverseMap();
            CreateMap<TaxInvoice, TaxInvoiceDTO>().ReverseMap();
            CreateMap<Ship, ShipDTO>().ReverseMap();
            
            CreateMap<Item, ItemDTO>().ReverseMap();
            CreateMap<Fpa, FpaDTO>().ReverseMap();
            CreateMap<Client, ClientDTO>().ReverseMap();
            CreateMap<ResponseDocResponse, MyDataResponseDTO>();
            CreateMap<ResponseDocResponse, MyDataCancelationResponseDTO>();
            CreateMap<ResponseDocResponseError, MyDataErrorDTO>();
            CreateMap<ResponseDocResponseError, MyDataCancelationErrorDTO>();
        }

       
    }
}

