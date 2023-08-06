
using Refit;
using System.ComponentModel.DataAnnotations;

namespace Postwomen.Interfaces;

public interface ICollection
{
	[Get("/")]
	Task<ApiResponse<string>> Get([Url] string url);

	//[Get("/api/tahsilatozet/getkullanicisubetahsilatozetbugunturdetayli")]//BU GÜN TOKEN
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuGunDetayliTokenli([Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getsubetahsilatozetbugunturdetayli/{company_id}/{branch_id}")]//BU GÜN -> SUBEID
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuGunDetayliWithBranchId(int company_id, int branch_id, [Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getsubetahsilatozetbugunturdetayli/{company_id}")]//BU GÜN -> ADMIN
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuGunDetayliWithCompanyId(int company_id, [Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getkullanicisubetahsilatozetbuhaftaturdetayli")]//BU HAFTA TOKEN
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuHaftaDetayliTokenli([Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getsubetahsilatozetbuhaftaturdetayli/{company_id}/{branch_id}")]//BU HAFTA -> SUBEID
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuHaftaDetayliWithBranchId(int company_id, int branch_id, [Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getsubetahsilatozetbuhaftaturdetayli/{company_id}")]//BU HAFTA -> ADMIN
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuHaftaDetayliWithCompanyId(int company_id, [Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getkullanicisubetahsilatozetbuayturdetayli")]//BU AY -> TOKEN
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuAyDetayliTokenli([Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getsubetahsilatozetbuayturdetayli/{company_id}/{branch_id}")]//BU AY -> SUBEID
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuAyDetayliWithBranchId(int company_id, int branch_id, [Header("Authorization")] string authorization);

	//[Get("/api/tahsilatozet/getsubetahsilatozetbuayturdetayli/{company_id}")]//BU AY -> companyid
	//Task<ApiResponse<CollectionSummary>> GetTahsilatOzetBuAyDetayliWithCompanyId(int company_id, [Header("Authorization")] string authorization);

	//[Post("/api/tahsilatozet/getsubetahsilatozetozeltarihbykullanici")]//TAHSİLAT ÖZEL TARİH -> USER
	//Task<ApiResponse<CollectionSummary>> PostReadDateSpecifiedTahsilatOzetWithToken(SpecifyDateModel specifyDate, [Header("Authorization")] string authorization);

	//[Post("/api/tahsilatozet/getsubetahsilatozetozeltarih")]//TAHSİLAT ÖZET TARİH -> SUBEID
	//Task<ApiResponse<CollectionSummary>> PostReadDateSpecifiedTahsilatOzetWithBranchId(SpecifyDateWIDModel specifyDateWithBranchId, [Header("Authorization")] string authorization);
}

[Headers("Content-Type:application/json")]
public interface IApi : ICollection { }