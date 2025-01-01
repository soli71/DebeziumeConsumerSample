CONTAINER_ALREADY_STARTED="CONTAINER_ALREADY_STARTED_PLACEHOLDER"
if [[ ! -e $CONTAINER_ALREADY_STARTED ]]; then
    touch $CONTAINER_ALREADY_STARTED
    echo "-- First container startup --"
	curl -i -X POST -H "Accept:application/json" -H  "Content-Type:application/json" http://debezium:8083/connectors/ -d  @debezium-connector-config.json
else
    echo "-- Not first container startup --"
fi

echo "--------------------- Start QUARKUS -------------------"
exec java -Dquarkus.http.host=0.0.0.0 -Djava.util.logging.manager=org.jboss.logmanager.LogManager -XX:+ExitOnOutOfMemoryError -cp . -jar /deployments/quarkus-run.jar