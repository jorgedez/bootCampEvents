# Global Integration Bootcamp Tenerife - Azure Event Grid disparando Service Bus

Basados en el fichero de Sources (response.json) deserializaremos el mismo (ejemplo de petición de hoteles, 500 registros).

#Entrada de la App

La entrada de la aplicación irá unida a un Azure Event Grid (Blob) que será disparado cuando se inserte o se borre un fichero en el BlobStorage asociado, este evento será capaz de reaccionar ante:
  - Microsoft.Storage.BlobDelete (Borrado)
  - Microsoft.Storage.BlobCreated (Creado)
  
  https://docs.microsoft.com/es-es/azure/event-grid/
  
  En el primer caso lo descartaremos aunque dejaremos constancia en el Log de ApplicationInsight y en el segundo caso, una vez tenida la acción y la ruta hacia el archivo, lo procesaremos, deserializaremos y filtraremos de la siguiente manera:
  
  - Hoteles LowCost (Precio minimo hab <= 200€)
  - Hoteles Premiere (Precio mayor de 200€)
  - Hoteles Luxury (Categoría del hotel = 5)
  
  Una vez los procesemos vía Linq, los enviaremos a distintas Queues (LowCost,Premiere,Luxury) y despacharemos sus funciones Service Bus Triggers para procesar las entregas, dejando constancia en ApplicationInsight.
  
  - Todos estas funciones llevaran también sus DeadLetters para notificar en caso de entrega defectuosa.
