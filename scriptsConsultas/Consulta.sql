SELECT * FROM fluxobolsa ff inner join acoes ac on ff.AcoesId = ac.Id	
where ac.Sigla = 'AALR3'

SELECT * FROM ACOES where acoesinfoid = 5

update ACOES set DataPregao = datapregao - interval 1 day where id<100

SELECT * FROM acoesinfo

ALTER TABLE acoesS MODIFY Volume Double

SELECT * FROM fluxobolsa ff inner join acoesInfo ac on ff.AcoesInfoId = ac.Id
where codCorretora in('040', '016')
order by data desc , Descricao 

SELECT ABS(SUM(ff.volume)), Descricao,Data ,VARD,acoes.datapregao 
 FROM fluxobolsa ff inner join acoesInfo ac 
on (ff.AcoesInfoId = ac.Id )
inner join Acoes acoes on(acoes.AcoesInfoId = ac.id and data = DataPregao)
where codCorretora in('040', '016') and DataPregao = '2021-04-22'
group by  Descricao, Data,acoes.volume, vard,acoes.DataPregao
having SUM(ff.volume) < 0 and ABS(SUM(ff.volume)) > acoes.volume * 0.05
order by data desc