SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE AveragePeriodFromVerifiedToPaid
	@DealerId AS int
AS
BEGIN
	SET NOCOUNT ON;

    select CAST(AVG(CAST(timediff AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS Average
	from 
	( 
		select distinct CommissionId,
				datediff (d, 
						(select ActionDate AS DateVerified
							from CommissionHistory CH
							where ExistingPaymentStatusId = 1
								and NewPaymentStatusId = 2
								and CH.CommissionId = CoHi.CommissionId
								and NewDealerId = @DealerId
						), 
						(select ActionDate AS DatePaid
								from CommissionHistory CCH
								where ExistingPaymentStatusId = 4
									and NewPaymentStatusId = 5
									and CCH.CommissionId = CoHi.CommissionId
									and NewDealerId = @DealerId
						)
				)AS timediff
		from CommissionHistory AS CoHi
	) AS NotNullResult 
	where timediff is not null
END
GO
