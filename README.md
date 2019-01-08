# BackendSupport
<h2>Backend Support Library</h2>

This solution contains two projects used in backend to process large files and help to some application communicate between them.

Projects:
<h3>Message Queue</h3>
This library supports message queue transaction based on MSQueue process. This library works with binary, xml and text files and allow to process simultaneous files at the same time using an array of queues handlers. Message queue works in FIFO queue, so it's not easy to process parallell transactions.

<h4>Classes</h4>
<b>MessageQueueProcessor:</b> this abstract class is the base of queue processing.

<b>MessageQueueReader:</b> This class reads the objects from message queue.

<b>MessageQueueWriter:</b> Writes message queue from external applications.


<h3>Inter Communication</h3>
This library help to the applications to broadcast message between them.

For each library we added the testing library. For testing we are using a repository, mocking support (MOQ) and unity framework.

<b>Broadcast Manager:</b> This class library helps to broadcast message between application.
