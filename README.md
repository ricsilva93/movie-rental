# MovieRental Exercise

This is a dummy representation of a movie rental system.
Can you help us fix some issues and implement missing features?

 * The app is throwing an error when we start, please help us. Also, tell us what caused the issue.
 => A singleton service cannot depend on a scoped service such as DbContext. By changing to addScoped , the issue was fixed.
 
 * The rental class has a method to save, but it is not async, can you make it async and explain to us what is the difference?
 => By making the method async, will not block the current thread until the db operation finishes. Using an async method with await, returns 
 the thread to the pool while de I/O operation (db) is happening. When the db operation is finished, it continues.
 This allows the server to handle more requests at the same time by not blocking threads on I/O operations.
 
 * Please finish the method to filter rentals by customer name, and add the new endpoint.
 => Added a new endpoint GET /Rental?customer=maria . This is an endpoit to GET the resource Rental filtered by its customer name, following REST conventions for resource filtering.
 
 * We noticed we do not have a table for customers, it is not good to have just the customer name in the rental.
   Can you help us add a new entity for this? Don't forget to change the customer name field to a foreign key, and fix your previous method!
   => Added a dedicated customer entity and , on rentals, replaced customerName with a foreign key customerId.
   
 * In the MovieFeatures class, there is a method to list all movies, tell us your opinion about it.
 => Several changes were made to improve the original method:
  - AsNoTracking: Since this is a read-only operation, AsNoTracking was used so EntityFramework doesn't track entities, improves read performance.
  - Sorting: Needed before pagination, to guarantee we always return consistent data per page.
  - Pagination: Avoid returning the full table. Returning a full table can become a performance issue when data increases. 
  - Async: will not block the current thread until the db operation finishes (ToListAsync)
  - Projecting to Dto: Added as an example. Avoid exposing the internal logic and decouple the API from persistency.
 Also, filtering can be another good option to decrease the amount of data being returned.
 
 * No exceptions are being caught in this api, how would you deal with these exceptions?
=> I would centralize the exception handling logic in a middleware or filter, log errors or return standardized responses (such as ProblemDetail). This can help ensuring consistent error contracts or 
avoiding duplicate try/catch logic. In this example, I've created a few exceptions and a global exception handler middleware for this purpose.

	## Challenge (Nice to have)
We need to implement a new feature in the system that supports automatic payment processing. Given the advancements in technology, it is essential to integrate multiple payment providers into our system.

Here are the specific instructions for this implementation:

* Payment Provider Classes:
    * In the "PaymentProvider" folder, you will find two classes that contain basic (dummy) implementations of payment providers. These can be used as a starting point for your work.
* RentalFeatures Class:
    * Within the RentalFeatures class, you are required to implement the payment processing functionality.
* Payment Provider Designation:
    * The specific payment provider to be used in a rental is specified in the Rental model under the attribute named "PaymentMethod".
* Extensibility:
    * The system should be designed to allow the addition of more payment providers in the future, ensuring flexibility and scalability.
* Payment Failure Handling:
    * If the payment method fails during the transaction, the system should prevent the creation of the rental record. In such cases, no rental should be saved to the database.
	=> In this example I've implemented a PaymentProviderResolver to resolve the payment provider implementations in runtime, based on the name input on the http request. 
	This will provide extensability and is designed according to SOLID principles. The implementation is based on interfaces, and it is possible to add a new payment provider by simply creating a new 
	payment provider class and implementing the IPaymentProvider interface. This will be dynamically discovered in runtime by the PaymentProviderResolver during the POST Rentals request.
	=> As for failure handling, In this example the request will fail when the payment provider inserted is not found or if the payment provider returns false on the Pay() method. On both cases, they are 
	throwing an exception that will be treated on the exception handling middleware. A different approach could be used: for instance, using a result object as return, with a succeeded true/false).
	This way there's no need to use exceptions to control flow.
